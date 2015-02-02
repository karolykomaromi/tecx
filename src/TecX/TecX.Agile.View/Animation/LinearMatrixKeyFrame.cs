using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TecX.Agile.View.Animation
{
    public class LinearMatrixKeyFrame : MatrixKeyFrame
    {
        #region c'tor

        public LinearMatrixKeyFrame()
        {
        }

        public LinearMatrixKeyFrame(Matrix value)
            : base(value)
        {
        }

        public LinearMatrixKeyFrame(Matrix value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion c'tor

        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new LinearMatrixKeyFrame();
        }

        #endregion Overrides of Freezable

        #region Overrides of MatrixKeyFrame

        protected override Matrix InterpolateValueCore(Matrix baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress == 0.0)
                return baseValue;

            if (keyFrameProgress == 1.0)
                return base.Value;

            return GeometryHelper.InterpolateMatrix(baseValue, base.Value, keyFrameProgress);
        }

        #endregion Overrides of MatrixKeyFrame
    }
}
