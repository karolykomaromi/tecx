using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using TecX.Common;

namespace TecX.Agile.Data.Xml
{
    /// <summary>
    /// Extension methods that convert <see cref="PlanningArtefact"/>s to and from <see cref="XElement"/>
    /// </summary>
    public static class ConversionExtensions
    {
        #region Constants

        private static class Constants
        {
            /// <summary>F2</summary>
            public const string Precision = "F2";

            /// <summary>Mapping</summary>
            public const string MappingElementName = "Mapping";
        }


        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region Conversion from XML

        /// <summary>
        /// Fills a <see cref="PlanningArtefact"/> with values from an XML serialized object
        /// </summary>
        /// <param name="planningArtefact">The planning artefact.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The <see cref="PlanningArtefact"/> filled with the values from an XML serialized
        /// object</returns>
        private static void FromXml(this PlanningArtefact planningArtefact, XElement xml)
        {
            Guard.AssertNotNull(planningArtefact, "planningArtefact");
            Guard.AssertNotNull(xml, "xml");

            string description;
            if (xml.TryGetValue(Agile.Constants.Properties.PlanningArtefact.Description, out description))
            {
                planningArtefact.Description = description;
            }

            Guid id;
            if (xml.TryGetValue(Agile.Constants.Properties.PlanningArtefact.Id, out id))
            {
                planningArtefact.Id = id;
            }

            string name;
            if (xml.TryGetValue(Agile.Constants.Properties.PlanningArtefact.Name, out name))
            {
                planningArtefact.Name = name;
            }

            return;
        }

        /// <summary>
        /// Fills an <see cref="StoryCardCollection"/> with values from an XML serialized object
        /// </summary>
        /// <param name="storyCardCollection">The planning artefact.</param>
        /// <param name="xml">The xml.</param>
        /// <returns>The <see cref="StoryCardCollection"/> filled with the values from an XML serialized
        /// object</returns>
        private static void FromXml(this StoryCardCollection storyCardCollection, XElement xml)
        {
            Guard.AssertNotNull(storyCardCollection, "storyCardCollection");
            Guard.AssertNotNull(xml, "xml");

            ((PlanningArtefact)storyCardCollection).FromXml(xml);

            IEnumerable<XElement> storycards = xml.Descendants(
                Agile.Constants.Properties.StoryCardCollection.StoryCards);

            foreach (XElement element in storycards.Descendants("StoryCard"))
            {
                var storycard = new StoryCard();
                storycard.FromXml(element);

                storyCardCollection.Add(storycard);
            }

            return;
        }

        /// <summary>
        /// Fills a <see cref="StoryCard"/> with values from an XML serialized object
        /// </summary>
        /// <param name="storycard">The storycard.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The <see cref="StoryCard"/> filled with the values from an XML serialized
        /// object</returns>
        public static void FromXml(this StoryCard storycard, XElement xml)
        {
            Guard.AssertNotNull(storycard, "storycard");
            Guard.AssertNotNull(xml, "xml");

            ((PlanningArtefact)storycard).FromXml(xml);

            string strCurrentSideUp;
            if (xml.TryGetValue(Agile.Constants.Properties.StoryCard.CurrentSideUp, out strCurrentSideUp))
            {
                var currentSideUp = Common.Convert.ToEnum<StoryCardSides>(strCurrentSideUp);
                storycard.CurrentSideUp = currentSideUp;
            }

            byte[] descriptionHandwritingImage;
            if (xml.TryGetValue(Agile.Constants.Properties.StoryCard.DescriptionHandwritingImage,
                                out descriptionHandwritingImage))
            {
                storycard.DescriptionHandwritingImage = descriptionHandwritingImage;
            }

            string owner;
            if (xml.TryGetValue(Agile.Constants.Properties.StoryCard.TaskOwner, out owner))
            {
                storycard.TaskOwner = owner;
            }

            storycard.Tracking.FromXml(xml);
            storycard.View.FromXml(xml);

            return;
        }

        /// <summary>
        /// Fills an <see cref="Iteration"/> with values from an XML serialized object
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The <see cref="Iteration"/> filled with the values from an XML serialized
        /// object</returns>
        public static void FromXml(this Iteration iteration, XElement xml)
        {
            Guard.AssertNotNull(iteration, "iteration");
            Guard.AssertNotNull(xml, "xml");

            ((StoryCardCollection)iteration).FromXml(xml);

            decimal availableEffort;
            if (xml.TryGetValue(Agile.Constants.Properties.Iteration.AvailableEffort, out availableEffort))
            {
                iteration.AvailableEffort = availableEffort;
            }

            DateTime endDate;
            if (xml.TryGetValue(Agile.Constants.Properties.Iteration.EndDate, out endDate))
            {
                iteration.EndDate = endDate;
            }

            DateTime startDate;
            if (xml.TryGetValue(Agile.Constants.Properties.Iteration.StartDate, out startDate))
            {
                iteration.StartDate = startDate;
            }

            iteration.Tracking.FromXml(xml);
            iteration.View.FromXml(xml);

            return;
        }

        /// <summary>
        /// Fills a <see cref="Visualizable"/> with values from an XML serialized object
        /// </summary>
        /// <param name="visualizable">The visualizable.</param>
        /// <param name="xml">The XML.</param>
        public static void FromXml(this Visualizable visualizable, XElement xml)
        {
            Guard.AssertNotNull(visualizable, "visualizable");
            Guard.AssertNotNull(xml, "xml");

            double x;
            if (xml.TryGetValue(Agile.Constants.Properties.Visualizable.X, out x))
            {
                visualizable.X = x;
            }

            double y;
            if (xml.TryGetValue(Agile.Constants.Properties.Visualizable.Y, out y))
            {
                visualizable.Y = y;
            }

            double width;
            if (xml.TryGetValue(Agile.Constants.Properties.Visualizable.Width, out width))
            {
                visualizable.Width = width;
            }

            double rotationAngle;
            if (xml.TryGetValue(Agile.Constants.Properties.Visualizable.RotationAngle, out rotationAngle))
            {
                visualizable.RotationAngle = rotationAngle;
            }

            byte[] color;
            if (xml.TryGetValue(Agile.Constants.Properties.Visualizable.Color, out color))
            {
                visualizable.Color = color;
            }

            double height;
            if (xml.TryGetValue(Agile.Constants.Properties.Visualizable.Height, out height))
            {
                visualizable.Height = height;
            }
        }

        /// <summary>
        /// Fills a <see cref="Trackable"/> with values from an XML serialized object
        /// </summary>
        /// <param name="trackable">The trackable.</param>
        /// <param name="xml">The XML.</param>
        public static void FromXml(this Trackable trackable, XElement xml)
        {
            Guard.AssertNotNull(trackable, "trackable");
            Guard.AssertNotNull(xml, "xml");

            decimal actualEffort;
            if (xml.TryGetValue(Agile.Constants.Properties.Trackable.ActualEffort, out actualEffort))
            {
                trackable.ActualEffort = actualEffort;
            }

            decimal bestCaseEstimate;
            if (xml.TryGetValue(Agile.Constants.Properties.Trackable.BestCaseEstimate,
                                out bestCaseEstimate))
            {
                trackable.BestCaseEstimate = bestCaseEstimate;
            }

            decimal mostLikelyEstimate;
            if (xml.TryGetValue(Agile.Constants.Properties.Trackable.MostLikelyEstimate,
                                out mostLikelyEstimate))
            {
                trackable.MostLikelyEstimate = mostLikelyEstimate;
            }

            string priorityString;
            if (xml.TryGetValue(Agile.Constants.Properties.Trackable.Priority, out priorityString))
            {
                Priority priority = Common.Convert.ToEnum<Priority>(priorityString);
                trackable.Priority = priority;
            }

            string statusString;
            if (xml.TryGetValue(Agile.Constants.Properties.Trackable.Status, out statusString))
            {
                Status status = Common.Convert.ToEnum<Status>(statusString);
                trackable.Status = status;
            }

            decimal worstCaseEstimate;
            if (xml.TryGetValue(Agile.Constants.Properties.Trackable.WorstCaseEstimate,
                                out worstCaseEstimate))
            {
                trackable.WorstCaseEstimate = worstCaseEstimate;
            }
        }

        public static void FromXml(this Legend legend, XElement xml)
        {
            Guard.AssertNotNull(legend, "legend");
            Guard.AssertNotNull(xml, "xml");

            foreach (XElement mapping in xml.Elements(Constants.MappingElementName))
            {
                string name;
                if (mapping.TryGetValue(Agile.Constants.Properties.Mapping.Name, out name))
                {
                    byte[] color;
                    if (mapping.TryGetValue(Agile.Constants.Properties.Mapping.Color, out color))
                    {
                        legend.Add(name, color);
                    }
                }
            }
        }

        /// <summary>
        /// Fills a <see cref="Backlog"/> with values from an XML serialized object
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The <see cref="Backlog"/> filled with the values from an XML serialized
        /// object</returns>
        public static void FromXml(this Backlog backlog, XElement xml)
        {
            Guard.AssertNotNull(backlog, "backlog");
            Guard.AssertNotNull(xml, "xml");

            ((StoryCardCollection)backlog).FromXml(xml);

            return;
        }

        /// <summary>
        /// Fills a <see cref="Project"/> with values from an XML serialized object
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The <see cref="Project"/> filled with the values from an XML serialized
        /// object</returns>
        public static void FromXml(this Project project, XElement xml)
        {
            Guard.AssertNotNull(project, "project");
            Guard.AssertNotNull(xml, "xml");

            ((PlanningArtefact)project).FromXml(xml);

            XElement xmlBacklog = xml.Element("Backlog");
            if (xmlBacklog != null)
            {
                var backlog = new Backlog();
                backlog.FromXml(xmlBacklog);

                project.Backlog = backlog;
            }

            XElement xmlLegend = xml.Element(Agile.Constants.Properties.Project.Legend);
            if (xmlLegend != null)
            {
                project.Legend.FromXml(xmlLegend);
            }

            IEnumerable<XElement> xmlIterations = xml.Descendants("Iteration");
            {
                foreach (XElement xmlIteration in xmlIterations)
                {
                    var iteration = new Iteration();
                    iteration.FromXml(xmlIteration);
                    project.Add(iteration);
                }
            }

            return;
        }

        #endregion Conversion from XML

        ////////////////////////////////////////////////////////////

        #region Conversion to XML

        /// <summary>
        /// Converts a <see cref="PlanningArtefact"/> to its XML serialized representation
        /// </summary>
        /// <param name="planningArtefact">The planning artefact.</param>
        /// <returns>
        /// The XML serialized representation of the <see cref="PlanningArtefact"/>
        /// </returns>
        private static XElement ToXml(this PlanningArtefact planningArtefact)
        {
            Guard.AssertNotNull(planningArtefact, "planningArtefact");

            var xml = new XElement(planningArtefact.GetType().Name);

            xml.AddAttribute(Agile.Constants.Properties.PlanningArtefact.Description,
                             TypeHelper.ToNullSafeString(planningArtefact.Description))
                .AddAttribute(Agile.Constants.Properties.PlanningArtefact.Id,
                              TypeHelper.ToNullSafeString(planningArtefact.Id, Guid.Empty.ToString()))
                .AddAttribute(Agile.Constants.Properties.PlanningArtefact.Name,
                              TypeHelper.ToNullSafeString(planningArtefact.Name));

            return xml;
        }

        public static XElement ToXml(this Legend legend)
        {
            Guard.AssertNotNull(legend, "legend");

            XElement xml = new XElement(Agile.Constants.Properties.Project.Legend);

            foreach (Mapping mapping in legend)
            {
                XElement map = new XElement(Constants.MappingElementName)
                    .AddAttribute(Agile.Constants.Properties.Mapping.Name, mapping.Name)
                    .AddAttribute(Agile.Constants.Properties.Mapping.Color, Common.Convert.ToHex(mapping.Color));

                xml.Add(map);
            }

            return xml;
        }

        /// <summary>
        /// Converts an <see cref="StoryCardCollection"/> to its XML serialized representation
        /// </summary>
        /// <param name="indexCardWithChildren">The index card with children.</param>
        /// <returns>
        /// The XML serialized representation of the <see cref="StoryCardCollection"/>
        /// </returns>
        private static XElement ToXml(this StoryCardCollection indexCardWithChildren)
        {
            Guard.AssertNotNull(indexCardWithChildren, "storyCardCollection");

            var xml = ((PlanningArtefact)indexCardWithChildren).ToXml();

            if (indexCardWithChildren.StoryCards != null &&
                indexCardWithChildren.StoryCards.Count() > 0)
            {
                XElement storyCards = new XElement(
                    Agile.Constants.Properties.StoryCardCollection.StoryCards);

                foreach (StoryCard storycard in indexCardWithChildren.StoryCards)
                {
                    var xmlStoryCard = storycard.ToXml();
                    storyCards.Add(xmlStoryCard);
                }

                xml.Add(storyCards);
            }

            return xml;
        }

        /// <summary>
        /// Converts a <see cref="StoryCard"/> to its XML serialized representation
        /// </summary>
        /// <param name="storycard">The storycard.</param>
        /// <returns>
        /// The XML serialized representation of the <see cref="StoryCard"/>
        /// </returns>
        public static XElement ToXml(this StoryCard storycard)
        {
            Guard.AssertNotNull(storycard, "storycard");

            var xml = ((PlanningArtefact)storycard).ToXml();

            xml.AddAttribute(Agile.Constants.Properties.StoryCard.CurrentSideUp,
                             Enum.GetName(typeof(StoryCardSides), storycard.CurrentSideUp))
                .AddAttribute(Agile.Constants.Properties.StoryCard.DescriptionHandwritingImage,
                              Common.Convert.ToHex(storycard.DescriptionHandwritingImage))
                .AddAttribute(Agile.Constants.Properties.StoryCard.TaskOwner, TypeHelper.ToNullSafeString(storycard.TaskOwner));

            storycard.View.ToXml(xml);
            storycard.Tracking.ToXml(xml);

            return xml;
        }

        /// <summary>
        /// Converts an <see cref="Iteration"/> to its XML serialized representation
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <returns>
        /// The XML serialized representation of the <see cref="Iteration"/>
        /// </returns>
        public static XElement ToXml(this Iteration iteration)
        {
            Guard.AssertNotNull(iteration, "iteration");

            var xml = ((StoryCardCollection)iteration).ToXml();

            xml.AddAttribute(Agile.Constants.Properties.Iteration.AvailableEffort,
                             iteration.AvailableEffort.ToString(
                                 Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Iteration.EndDate,
                              iteration.EndDate.ToString("o", CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Iteration.StartDate,
                              iteration.StartDate.ToString("o", CultureInfo.InvariantCulture));

            iteration.View.ToXml(xml);
            iteration.Tracking.ToXml(xml);

            return xml;
        }

        /// <summary>
        /// Converts a <see cref="Backlog"/> to its XML serialized representation
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        /// <returns>
        /// The XML serialized representation of the <see cref="Backlog"/>
        /// </returns>
        public static XElement ToXml(this Backlog backlog)
        {
            Guard.AssertNotNull(backlog, "backlog");

            var xml = ((StoryCardCollection)backlog).ToXml();

            return xml;
        }

        /// <summary>
        /// Converts a <see cref="Project"/> to its XML serialized representation
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        /// The XML serialized representation of the <see cref="Project"/>
        /// </returns>
        public static XElement ToXml(this Project project)
        {
            Guard.AssertNotNull(project, "project");

            var xml = ((PlanningArtefact)project).ToXml();

            if (project.Legend != null &&
                project.Legend.Count > 0)
            {
                var legendElement = project.Legend.ToXml();
                xml.Add(legendElement);
            }

            if (project.Backlog != null)
            {
                var xmlBacklog = project.Backlog.ToXml();
                xml.Add(xmlBacklog);
            }

            if (project.Iterations != null &&
                project.Iterations.Count() > 0)
            {
                foreach (Iteration iteration in project.Iterations)
                {
                    var xmlIteration = iteration.ToXml();
                    xml.Add(xmlIteration);
                }
            }

            return xml;
        }

        /// <summary>
        /// Converts a <see cref="Visualizable"/> to its XML serialized representation
        /// </summary>
        /// <param name="visualizable">The visualizable.</param>
        /// <param name="xml">The XML.</param>
        public static void ToXml(this Visualizable visualizable, XElement xml)
        {
            Guard.AssertNotNull(visualizable, "visualizable");
            Guard.AssertNotNull(xml, "xml");

            xml.AddAttribute(Agile.Constants.Properties.Visualizable.X,
                             visualizable.X.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Visualizable.Y,
                              visualizable.Y.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Visualizable.RotationAngle,
                              visualizable.RotationAngle.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Visualizable.Width,
                              visualizable.Width.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Visualizable.Height,
                              visualizable.Height.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Visualizable.Color,
                              Common.Convert.ToHex(visualizable.Color));
        }

        /// <summary>
        /// Converts a <see cref="Trackable"/> to its XML serialized representation
        /// </summary>
        /// <param name="trackable">The trackable.</param>
        /// <param name="xml">The XML.</param>
        public static void ToXml(this Trackable trackable, XElement xml)
        {
            Guard.AssertNotNull(trackable, "trackable");
            Guard.AssertNotNull(xml, "xml");

            xml.AddAttribute(Agile.Constants.Properties.Trackable.ActualEffort,
                             trackable.ActualEffort.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Trackable.BestCaseEstimate,
                              trackable.BestCaseEstimate.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Trackable.MostLikelyEstimate,
                              trackable.MostLikelyEstimate.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddAttribute(Agile.Constants.Properties.Trackable.Priority,
                              TypeHelper.ToNullSafeString(trackable.Priority))
                .AddAttribute(Agile.Constants.Properties.Trackable.Status,
                              TypeHelper.ToNullSafeString(trackable.Status))
                .AddAttribute(Agile.Constants.Properties.Trackable.WorstCaseEstimate,
                              trackable.WorstCaseEstimate.ToString(Constants.Precision, CultureInfo.InvariantCulture));
        }

        #endregion Conversion to XML
    }
}
