using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using TecX.Agile.Infrastructure;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class TossingBehavior : MovementBehaviorBase
    {
        private static class Constants
        {
            /// <summary>AnimatedTranslateTransform</summary>
            public const string TranslationAnimationName = "AnimatedTranslateTransform";

            /// <summary>AnimatedRotateTransform</summary>
            public const string RotationAnimationName = "AnimatedRotateTransform";

            /// <summary>TossingAnimation</summary>
            public const string TossingAnimation = "TossingAnimation";

            /// <summary>3</summary>
            public const int RecentPointBufferSize = 3;

            /// <summary>10</summary>
            public const double StaticFrictionCoefficient = 10;

            /// <summary>5.5</summary>
            public const double DecelerationConstant = 5.5;

            /// <summary>0.1</summary>
            public const double TossingCutoff = 0.1;

            /// <summary>0.065</summary>
            public const double CurrentFriction = 0.065;

            //friction to eventually make the tossing stop
            public const double FrictionModifier = (1 - CurrentFriction);
        }

        #region Fields

        private readonly IEventAggregator _eventAggregator;
        
        /// <summary>
        /// Gets the end time of the mouse movement
        /// </summary>
        /// <remarks>
        /// Used for calculating the duration of the tossing move
        /// </remarks>
        private DateTime _endTime;

        /// <summary>
        /// Gets the start time of the mouse movement
        /// </summary>
        /// <remarks>
        /// Used for calculating the duration of the tossing move
        /// </remarks>
        private DateTime _startTime;

        /// <summary>
        /// Gets and sets the <see cref="Storyboard"/> that coordinates the tossing animations
        /// </summary>
        private Storyboard _storyboard;

        /// <summary>
        /// Gets and sets the vector between the coordinates of the first mouse
        /// event and the center of the tossed item
        /// </summary>
        private Vector _initialVector;

        /// <summary>
        /// Gets and sets the vector along which the item is tossed
        /// </summary>
        private Vector _tossingVector;

        /// <summary>
        /// Gets and sets the angle of rotation at the time the item was tossed
        /// </summary>
        private double _initialAngle;

        /// <summary>
        /// Gets the list of the last coordinates where relevant mouse-events occured
        /// </summary>
        private readonly List<Point> _recentPoints;

        /// <summary>
        /// Gets and sets flag indicating wether the item is currently tossed
        /// </summary>
        private bool _isTossed;

        private MatrixAnimationUsingKeyFrames _tossingAnimation;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="TossingBehavior"/> class.
        /// </summary>
        public TossingBehavior()
        {
            _eventAggregator = EventAggregatorAccessor.EventAggregator;

            _recentPoints = new List<Point>();
            _isTossed = false;
        }

        #endregion c'tor

        protected override void OnAttached()
        {
            base.OnAttached();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove += OnPreviewMouseMove;

            InitializeAnimations();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.UnregisterName(Constants.RotationAnimationName);

            AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove -= OnPreviewMouseMove;
        }

        #region EventHandler

        private void OnStoryboardCompleted(object sender, EventArgs e)
        {
            //TODO weberse must use matrixtransform
            //if (_translationXAnimation.KeyFrames.Count > 0)
            //    AssociatedObject.Translation().X = _translationXAnimation.KeyFrames[_translationXAnimation.KeyFrames.Count - 1].Value;

            //if (_translationYAnimation.KeyFrames.Count > 0)
            //    AssociatedObject.Translation().Y = _translationYAnimation.KeyFrames[_translationYAnimation.KeyFrames.Count - 1].Value;

            ////if the card was only translated you don't have to update the rotation angle!
            //if (_rotationAnimation.KeyFrames.Count > 0)
            //    AssociatedObject.Rotation().Angle = _rotationAnimation.KeyFrames[_rotationAnimation.KeyFrames.Count - 1].Value;

            //clear the keyframes, they are no longer needed
            ClearAnimationKeyFrames();

            //fire event
            _eventAggregator.Publish(new ItemDropped(AssociatedObject, AssociatedObject.PointToScreen(AssociatedObject.Center())));
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point actual = e.GetPosition(Tabletop.Surface);

            if (e.LeftButton.Equals(MouseButtonState.Pressed) && AssociatedObject.IsMouseCaptured)
            {
                //used for keeping track of the movement of the card
                AddMousePoint(actual);
            }
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddMousePoint(e.GetPosition(Tabletop.Surface));

            //check if the movement was fast enough to count as tossing
            double length = (_recentPoints[0] - _recentPoints.Last()).Length;

            if (length > Constants.StaticFrictionCoefficient)
            {
                _isTossed = true;

                //vector along which to move the card
                _tossingVector = _recentPoints.Last() - _recentPoints[0];

                //setting up the vector for the initial moment of the tossing RNT
                _initialVector = _recentPoints.Last() - AssociatedObject.CenterOnSurface();
                
                _initialAngle = GeometryHelper.GetRotationAngleFromMatrix(AssociatedObject.Transform().Matrix);

                _endTime = DateTime.Now;

                if (!MovementBehaviorBase.GetIsPinned(AssociatedObject))
                {
                    //TODO tossing strategy does not have toa -> assure toa is there?
                    double distanceFromCenter = GeometryHelper.GetDistanceBetween(AssociatedObject.Center(),
                                                                                  e.GetPosition(AssociatedObject));

                    if (distanceFromCenter < TranslateOnlyArea.DefaultRadius)
                    {
                        ApplyTranslateOnlyTossing();
                    }
                    else
                    {
                        ApplyRntTossing();
                    }
                }
            }

            //make sure that we don't have any old points in our watchlist
            _recentPoints.Clear();
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point actual = e.GetPosition(Tabletop.Surface);

            //if the card is currently in an animated movement this saves
            //the current position of the card and fires the DropEvent to notify
            //the application of the position update
            if (_storyboard.GetCurrentState(AssociatedObject) == ClockState.Active)
            {
                //Yippie-Ki-Yay ... stops the card from jumping if
                //you click on label or resizerhandle
                //TODO weberse use tabletop here?
                Point dropPoint = Application.Current.MainWindow.PointToScreen(actual);

                _eventAggregator.Publish(new ItemDropped(AssociatedObject, dropPoint));

                //don't forget to stop the animation
                _storyboard.Stop(AssociatedObject);
            }

            //clear the list of keyframes or you'll get unexpected
            //results
            ClearAnimationKeyFrames();

            _isTossed = false;

            _startTime = DateTime.Now;

            _recentPoints.Clear();
            AddMousePoint(actual);
        }

        #endregion EventHandler



        #region Initialization

        private void InitializeAnimations()
        {
            NameScope.SetNameScope(AssociatedObject, new NameScope());

            _storyboard = new Storyboard();

            //its easier to access the translation if you
            //give it a name

            //TODO weberse must use matrixtransform
            AssociatedObject.RegisterName(Constants.TossingAnimation, AssociatedObject.Transform());

            _tossingAnimation = new MatrixAnimationUsingKeyFrames();

            //if you want to access and change a value after its animation
            //you have to choose FillBehavior.Stop
            _tossingAnimation.FillBehavior = FillBehavior.Stop;

            //link the animation with the property it should change (here the X and Y values
            //of the translation and the rotation angle of the card)
            Storyboard.SetTargetName(_tossingAnimation, Constants.TossingAnimation);
            Storyboard.SetTargetProperty(_tossingAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            //don't forget to add the animations to the Storyboard or nothin
            //will happen
            _storyboard.Children.Add(_tossingAnimation);

            _storyboard.Begin(AssociatedObject, true);
            _storyboard.Stop(AssociatedObject);

            _storyboard.Completed += OnStoryboardCompleted;
        }

        private void AddMousePoint(Point actual)
        {
            //add the current mouse position to the list
            //of recorded coordinates for tossing
            if (_recentPoints.Count <= Constants.RecentPointBufferSize)
            {
                _recentPoints.Add(actual);
            }
            else
            {
                _recentPoints.RemoveAt(0);
                _recentPoints.Add(actual);
            }
        }

        private void ApplyTranslateOnlyTossing()
        {
            //clear the list of keyframes or you'll get unexpected
            //results
            ClearAnimationKeyFrames();

            //for the animation we need to keep a local copy
            //of the card center point that we can move
            Point localCenter = AssociatedObject.CenterOnSurface();

            //movement is relative to the last position
            //Point previousPoint = new Point(AssociatedObject.Translation().X, AssociatedObject.Translation().Y);
            Point previousPoint = new Point(AssociatedObject.Transform().Matrix.OffsetX, AssociatedObject.Transform().Matrix.OffsetY);

            double stoppingDistance = 0;

            //as long as the tossing isnt aborted or the movement is unnoticeably small
            while (_tossingVector.Length >= Constants.TossingCutoff && _isTossed)
            {
                //slow down the movement
                _tossingVector *= Constants.FrictionModifier;

                Point actualPoint = previousPoint + _tossingVector;

                //TODO weberse 2010-12-20 must use last keyframe as offset or matrixtransform if first keyframe
                int count;
                Matrix matrix;
                if ((count = _tossingAnimation.KeyFrames.Count) == 0)
                {
                    matrix = AssociatedObject.Transform().Matrix;
                }
                else
                {
                    MatrixKeyFrame lastKeyFrame = _tossingAnimation.KeyFrames[count - 1];
                    matrix = lastKeyFrame.Value;
                }

                //TODO weberse 2010-12-20 do i have to make this analog to FrameworkElementExtensions.Move and divide it into separate
                //steps in order not to mess up the change tracking?
                matrix.Translate(_tossingVector.X, _tossingVector.Y);
                _tossingAnimation.KeyFrames.Add(new DiscreteMatrixKeyFrame(matrix));

                stoppingDistance += (previousPoint - actualPoint).Length;

                previousPoint = actualPoint;

                //don't forget to move the card center
                localCenter += _tossingVector;

                //check if the card remains within window bounds
                //TODO refactor to use PointTo/FromScreen
                if (
                    GeometryHelper.IsRelativePointOutsideCanvas(localCenter, Tabletop.Surface) &&
                    _isTossed)
                {
                    localCenter = ResetInvalidTossingMove(localCenter);

                    _isTossed = false;
                }
            }

            _isTossed = false;

            TimeSpan animationTime = CalculateAnimationTime(stoppingDistance);
            
            _tossingAnimation.Duration = animationTime;

            //only start the animation if the card would actually move but allow us to
            //interupt it)
            if (stoppingDistance > 0)
                _storyboard.Begin(AssociatedObject, true);
        }

        private void ApplyRntTossing()
        {
            //clear the list of keyframes or you'll get unexpected
            //results
            ClearAnimationKeyFrames();

            //for the animation we need to keep a local copy
            //of the card center point that we can move
            Point localCenter = AssociatedObject.CenterOnSurface();
            //double localRotationAngle = AssociatedObject.Rotation().Angle;
            double localRotationAngle = GeometryHelper.GetRotationAngleFromMatrix(AssociatedObject.Transform().Matrix);

            //movement is relative to the last position
            Point previousPoint = AssociatedObject.CenterOnSurface();

            double stoppingDistance = 0;


            //as long as the tossing isnt aborted or the movement is unnoticeably small
            while ((_tossingVector.Length >= Constants.TossingCutoff) && _isTossed)
            {
                //slow down the movement
                _tossingVector *= Constants.FrictionModifier;

                double angleDiff = GeometryHelper.ToRadians(localRotationAngle - _initialAngle);
                double sine = Math.Sin(angleDiff);
                double cosine = Math.Cos(angleDiff);

                double rx = (_initialVector.X * cosine - _initialVector.Y * sine);
                double ry = (_initialVector.X * sine + _initialVector.Y * cosine);

                previousPoint.X = localCenter.X + rx;
                previousPoint.Y = localCenter.Y + ry;

                CalculateRntSteps(localCenter, previousPoint);

                stoppingDistance += _tossingVector.Length;

                //don't forget to move the card center
                localCenter += _tossingVector;

                //check if the card remains within window bounds
                //TODO refactor to use PointTo/FromScreen
                if (
                    GeometryHelper.IsRelativePointOutsideCanvas(localCenter, Tabletop.Surface) &&
                    _isTossed)
                {
                    localCenter = ResetInvalidTossingMove(localCenter);

                    _isTossed = false;
                }

                //TODO the corners are NOT really moved so they need to be updated somehow!
                //TODO refactor to use PointTo/FromScreen
                if (
                    GeometryHelper.IsRelativePointOutsideCanvas(localCenter, Tabletop.Surface) &&
                    _isTossed)
                {
                    ResetInvalidTossingMove(localCenter);
                    _isTossed = false;
                }
            }

            _isTossed = false;

            //if the card stops because it hits a window border the time is
            //not calculated properly and the movement might appear unnaturaly slow
            TimeSpan animationTime = CalculateAnimationTime(stoppingDistance);

            _tossingAnimation.Duration = animationTime;

            //only start the animation if the card would actually move but allow us to
            //interupt it
            if (stoppingDistance > 0)
                _storyboard.Begin(AssociatedObject, true);
        }

        private Point ResetInvalidTossingMove(Point point)
        {
            int last = _tossingAnimation.KeyFrames.Count - 1;

            if (last > -1)
            {
                Vector displacement = GeometryHelper.GetPointOutsideShapeDisplacement(point, Tabletop.Surface);

                Matrix lastValid = _tossingAnimation.KeyFrames[last].Value;

                lastValid.Translate(displacement.X, displacement.Y);

                _tossingAnimation.KeyFrames.RemoveAt(last);
                _tossingAnimation.KeyFrames.Add(new DiscreteMatrixKeyFrame(lastValid));

                point += displacement;
            }

            return point;
        }

        //TODO weberse same as in GeometryHelper?
        private void CalculateRntSteps(Point localCenter, Point previousPoint)
        {
            Vector vStart = previousPoint - localCenter;

            Vector vEnd = previousPoint - localCenter + _tossingVector;

            double angle = Vector.AngleBetween(vStart, vEnd) % 360;

            double scalar = (vEnd.Length - vStart.Length) / vEnd.Length;

            Vector vDisplacement = vEnd * scalar;

            int count;
            Matrix matrix;
            if ((count = _tossingAnimation.KeyFrames.Count) == 0)
            {
                matrix = AssociatedObject.Transform().Matrix;
            }
            else
            {
                MatrixKeyFrame keyFrame = _tossingAnimation.KeyFrames[count - 1];
                matrix = keyFrame.Value;
            }

            matrix.Translate(vDisplacement.X, vDisplacement.Y);
            matrix.RotateAt(angle, localCenter.X, localCenter.Y);

            _tossingAnimation.KeyFrames.Add(new DiscreteMatrixKeyFrame(matrix));
        }

        private TimeSpan CalculateAnimationTime(double stoppingDistance)
        {
            //calculate the duration of the animation based on the speed of the
            //tossing gesture
            const double tps = TimeSpan.TicksPerSecond;
            double ticks = (_endTime - _startTime).Ticks;
            double time = ticks / tps;
            double s0 = GeometryHelper.GetDistanceBetween(_recentPoints[0], _recentPoints[_recentPoints.Count - 1]);

            //prohibits a divide-by-zero error
            double v0 = s0 / (time != 0 ? time : 1);

            double duration = (stoppingDistance * Constants.DecelerationConstant / v0) * 2 * Constants.CurrentFriction;

            return TimeSpan.FromSeconds(duration);
        }

        private void ClearAnimationKeyFrames()
        {
            _tossingAnimation.KeyFrames.Clear();
        }

        #endregion
    }
}
