using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Newtonsoft.Json.Linq;

using TecX.Common;

namespace TecX.Agile.Data.Json
{
    /// <summary>
    /// Extension methods that convert <see cref="PlanningArtefact"/>s to and from <see cref="JObject"/>
    /// </summary>
    public static class ConversionExtensions
    {
        #region Constants

        private static class Constants
        {
            /// <summary>F2</summary>
            public const string Precision = "F2";
        }

        #endregion Constants

        #region Conversion from JSON

        /// <summary>
        /// Fills a <see cref="PlanningArtefact"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="planningArtefact">The planning artefact.</param>
        /// <param name="json">The json.</param>
        /// <returns>The <see cref="PlanningArtefact"/> filled with the values from a JSON serialized
        /// object</returns>
        private static void FromJson(this PlanningArtefact planningArtefact, JObject json)
        {
            Guard.AssertNotNull(planningArtefact, "planningArtefact");
            Guard.AssertNotNull(json, "json");

            string description;
            if (json.TryGetValue(Agile.Constants.Properties.PlanningArtefact.Description, out description))
            {
                planningArtefact.Description = description;
            }

            Guid id;
            if (json.TryGetValue(Agile.Constants.Properties.PlanningArtefact.Id, out id))
            {
                planningArtefact.Id = id;
            }

            string name;
            if (json.TryGetValue(Agile.Constants.Properties.PlanningArtefact.Name, out name))
            {
                planningArtefact.Name = name;
            }

            return;
        }

        /// <summary>
        /// Fills an <see cref="StoryCardCollection"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="storyCardCollection">The index card with children.</param>
        /// <param name="json">The json.</param>
        /// <returns>The <see cref="StoryCardCollection"/> filled with the values from a JSON serialized
        /// object</returns>
        private static void FromJson(this StoryCardCollection storyCardCollection, JObject json)
        {
            Guard.AssertNotNull(storyCardCollection, "storyCardCollection");
            Guard.AssertNotNull(json, "json");

            ((PlanningArtefact) storyCardCollection).FromJson(json);

            JArray jsonStoryCards;
            if (json.TryGetValue(Agile.Constants.Properties.StoryCardCollection.StoryCards, out jsonStoryCards))
            {
                if (jsonStoryCards != null)
                {
                    foreach (JObject jsonStoryCard in jsonStoryCards)
                    {
                        if (jsonStoryCard != null)
                        {
                            var storycard = new StoryCard();
                            storycard.FromJson(jsonStoryCard);
                            storyCardCollection.Add(storycard);
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Fills a <see cref="StoryCard"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="storycard">The storycard.</param>
        /// <param name="json">The json.</param>
        /// <returns>The <see cref="StoryCard"/> filled with the values from a JSON serialized
        /// object</returns>
        public static void FromJson(this StoryCard storycard, JObject json)
        {
            Guard.AssertNotNull(storycard, "storycard");
            Guard.AssertNotNull(json, "json");

            ((PlanningArtefact) storycard).FromJson(json);

            string strCurrentSide;
            if (json.TryGetValue(Agile.Constants.Properties.StoryCard.CurrentSideUp, out strCurrentSide))
            {
                var currentSideUp = Common.Convert.ToEnum<StoryCardSides>(strCurrentSide);

                storycard.CurrentSideUp = currentSideUp;
            }

            byte[] descriptionHandwritingImage;
            if (json.TryGetValue(Agile.Constants.Properties.StoryCard.DescriptionHandwritingImage,
                                 out descriptionHandwritingImage))
            {
                storycard.DescriptionHandwritingImage = descriptionHandwritingImage;
            }

            string owner;
            if (json.TryGetValue(Agile.Constants.Properties.StoryCard.TaskOwner, out owner))
            {
                storycard.TaskOwner = owner;
            }

            storycard.Tracking.FromJson(json);
            storycard.View.FromJson(json);

            return;
        }

        /// <summary>
        /// Fills an <see cref="Iteration"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <param name="json">The json.</param>
        /// <returns>The <see cref="StoryCard"/> filled with the values from a JSON serialized
        /// object</returns>
        public static void FromJson(this Iteration iteration, JObject json)
        {
            Guard.AssertNotNull(iteration, "iteration");
            Guard.AssertNotNull(json, "json");

            ((StoryCardCollection) iteration).FromJson(json);

            decimal availableEffort;
            if (json.TryGetValue(Agile.Constants.Properties.Iteration.AvailableEffort, out availableEffort))
            {
                iteration.AvailableEffort = availableEffort;
            }

            DateTime endDate;
            if (json.TryGetValue(Agile.Constants.Properties.Iteration.EndDate, out endDate))
            {
                iteration.EndDate = endDate;
            }

            DateTime startDate;
            if (json.TryGetValue(Agile.Constants.Properties.Iteration.StartDate, out startDate))
            {
                iteration.StartDate = startDate;
            }

            iteration.Tracking.FromJson(json);
            iteration.View.FromJson(json);

            return;
        }

        /// <summary>
        /// Fills a <see cref="Backlog"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        /// <param name="json">The json.</param>
        /// <returns>The <see cref="Backlog"/> filled with the values from a JSON serialized
        /// object</returns>
        public static void FromJson(this Backlog backlog, JObject json)
        {
            Guard.AssertNotNull(backlog, "backlog");
            Guard.AssertNotNull(json, "json");

            ((StoryCardCollection) backlog).FromJson(json);

            return;
        }

        /// <summary>
        /// Fills a <see cref="Project"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="json">The json.</param>
        /// <returns>The <see cref="Project"/> filled with the values from a JSON serialized
        /// object</returns>
        public static void FromJson(this Project project, JObject json)
        {
            Guard.AssertNotNull(project, "project");
            Guard.AssertNotNull(json, "json");

            ((PlanningArtefact) project).FromJson(json);

            var jsonLegend = json[Agile.Constants.Properties.Project.Legend] as JObject;

            if (jsonLegend != null)
            {
                IEnumerable<JProperty> properties = jsonLegend.Properties();
                if (properties != null &&
                    properties.Count() > 0)
                {
                    foreach (JProperty prop in properties)
                    {
                        string key = prop.Name;

                        //TODO this sucks. make it robust
                        string str = ((JValue) prop.Value).ToString().Trim('\"');

                        byte[] value = Common.Convert.ToByte(str);

                        if (!string.IsNullOrEmpty(key) &&
                            value.Length > 0)
                        {
                            project.Legend.Add(key, value);
                        }
                    }
                }
            }

            var jsonBacklog = json[Agile.Constants.Properties.Project.Backlog] as JObject;

            if (jsonBacklog != null)
            {
                var backlog = new Backlog();
                backlog.FromJson(jsonBacklog);
                project.Backlog = backlog;
            }

            JArray jsonIterations;
            if (json.TryGetValue(Agile.Constants.Properties.Project.Iterations, out jsonIterations))
            {
                if (jsonIterations != null)
                {
                    foreach (JObject jsonIteration in jsonIterations)
                    {
                        var iteration = new Iteration();
                        iteration.FromJson(jsonIteration);
                        project.Add(iteration);
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Fills a <see cref="Visualizable"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="visualizable">The visualizable.</param>
        /// <param name="json">The json.</param>
        public static void FromJson(this Visualizable visualizable, JObject json)
        {
            Guard.AssertNotNull(visualizable, "visualizable");
            Guard.AssertNotNull(json, "json");

            byte[] color;
            if (json.TryGetValue(Agile.Constants.Properties.Visualizable.Color, out color))
            {
                visualizable.Color = color;
            }

            double height;
            if (json.TryGetValue(Agile.Constants.Properties.Visualizable.Height, out height))
            {
                visualizable.Height = height;
            }

            double rotationAngle;
            if (json.TryGetValue(Agile.Constants.Properties.Visualizable.RotationAngle, out rotationAngle))
            {
                visualizable.RotationAngle = rotationAngle;
            }

            double width;
            if (json.TryGetValue(Agile.Constants.Properties.Visualizable.Width, out width))
            {
                visualizable.Width = width;
            }

            double x;
            if (json.TryGetValue(Agile.Constants.Properties.Visualizable.X, out x))
            {
                visualizable.X = x;
            }

            double y;
            if (json.TryGetValue(Agile.Constants.Properties.Visualizable.Y, out y))
            {
                visualizable.Y = y;
            }
        }

        /// <summary>
        /// Fills a <see cref="Visualizable"/> with values from a JSON serialized object
        /// </summary>
        /// <param name="trackable">The trackable.</param>
        /// <param name="json">The json.</param>
        public static void FromJson(this Trackable trackable, JObject json)
        {
            Guard.AssertNotNull(trackable, "trackable");
            Guard.AssertNotNull(json, "json");

            decimal actualEffort;
            if (json.TryGetValue(Agile.Constants.Properties.Trackable.ActualEffort, out actualEffort))
            {
                trackable.ActualEffort = actualEffort;
            }

            decimal bestCaseEstimate;
            if (json.TryGetValue(Agile.Constants.Properties.Trackable.BestCaseEstimate, out bestCaseEstimate))
            {
                trackable.BestCaseEstimate = bestCaseEstimate;
            }

            decimal mostLikelyEstimate;
            if (json.TryGetValue(Agile.Constants.Properties.Trackable.MostLikelyEstimate, out mostLikelyEstimate))
            {
                trackable.MostLikelyEstimate = mostLikelyEstimate;
            }

            string priorityString;
            if (json.TryGetValue(Agile.Constants.Properties.Trackable.Priority, out priorityString))
            {
                Priority priority = Common.Convert.ToEnum<Priority>(priorityString);
                trackable.Priority = priority;
            }

            string statusString;
            if (json.TryGetValue(Agile.Constants.Properties.Trackable.Status, out statusString))
            {
                Status status = Common.Convert.ToEnum<Status>(statusString);
                trackable.Status = status;
            }

            decimal worstCaseEstimate;
            if (json.TryGetValue(Agile.Constants.Properties.Trackable.WorstCaseEstimate, out worstCaseEstimate))
            {
                trackable.WorstCaseEstimate = worstCaseEstimate;
            }
        }

        public static void FromJson(this Legend legend, JObject json)
        {
            Guard.AssertNotNull(legend, "legend");
            Guard.AssertNotNull(json, "json");

            legend.Clear();

            JProperty property = json.Property(Agile.Constants.Properties.Project.Legend);

            if (property != null)
            {
                JArray array = property.Value as JArray;

                if (array != null)
                {
                    foreach (JToken token in array)
                    {
                        JObject mapping = token as JObject;

                        if (mapping != null)
                        {
                            string name;
                            mapping.Property(Agile.Constants.Properties.Mapping.Name).Value.TryGetValue(out name);
                            byte[] color;
                            mapping.Property(Agile.Constants.Properties.Mapping.Color).Value.TryGetValue(out color);

                            if (!string.IsNullOrEmpty(name) && color != null && color.Count() == 4)
                            {
                                legend.Add(name, color);
                            }
                        }
                    }
                }
            }
        }

        #endregion Conversion from JSON

        #region Conversion to JSON

        /// <summary>
        /// Converts a <see cref="PlanningArtefact"/> to its JSON serialized representation
        /// </summary>
        /// <param name="planningArtefact">The planning artefact.</param>
        /// <returns>The JSON serialized representation of the <see cref="PlanningArtefact"/></returns>
        private static JObject ToJson(this PlanningArtefact planningArtefact)
        {
            Guard.AssertNotNull(planningArtefact, "planningArtefact");

            var json = new JObject();

            json.AddProperty(Agile.Constants.Properties.PlanningArtefact.Description,
                             TypeHelper.ToNullSafeString(planningArtefact.Description))
                .AddProperty(Agile.Constants.Properties.PlanningArtefact.Id,
                             TypeHelper.ToNullSafeString(planningArtefact.Id))
                .AddProperty(Agile.Constants.Properties.PlanningArtefact.Name,
                             TypeHelper.ToNullSafeString(planningArtefact.Name));

            return json;
        }

        /// <summary>
        /// Converts an <see cref="StoryCardCollection"/> to its JSON serialized representation
        /// </summary>
        /// <param name="indexCardWithChildren">The index card with children.</param>
        /// <returns>The JSON serialized representation of the <see cref="StoryCardCollection"/></returns>
        private static JObject ToJson(this StoryCardCollection indexCardWithChildren)
        {
            Guard.AssertNotNull(indexCardWithChildren, "storyCardCollection");

            var json = ((PlanningArtefact) indexCardWithChildren).ToJson();

            JArray storycards = new JArray();

            if (indexCardWithChildren.StoryCards != null &&
                indexCardWithChildren.StoryCards.Count() > 0)
            {
                foreach (StoryCard storycard in indexCardWithChildren.StoryCards)
                {
                    var jsonStoryCard = storycard.ToJson();
                    storycards.Add(jsonStoryCard);
                }
            }

            json.AddProperty(Agile.Constants.Properties.StoryCardCollection.StoryCards, storycards);

            return json;
        }

        /// <summary>
        /// Converts a <see cref="StoryCard"/> to its JSON serialized representation
        /// </summary>
        /// <param name="storycard">The storycard.</param>
        /// <returns>
        /// The JSON serialized representation of the <see cref="StoryCard"/>
        /// </returns>
        public static JObject ToJson(this StoryCard storycard)
        {
            Guard.AssertNotNull(storycard, "storycard");

            var json = ((PlanningArtefact) storycard).ToJson();

            json.AddProperty(Agile.Constants.Properties.StoryCard.CurrentSideUp,
                             TypeHelper.ToNullSafeString(storycard.CurrentSideUp))
                .AddProperty(Agile.Constants.Properties.StoryCard.DescriptionHandwritingImage,
                             Common.Convert.ToHex(storycard.DescriptionHandwritingImage))
                .AddProperty(Agile.Constants.Properties.StoryCard.TaskOwner,
                             TypeHelper.ToNullSafeString(storycard.TaskOwner));

            storycard.Tracking.ToJson(json);
            storycard.View.ToJson(json);

            return json;
        }

        /// <summary>
        /// Converts an <see cref="Iteration"/> to its JSON serialized representation
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <returns>
        /// The JSON serialized representation of the <see cref="Iteration"/>
        /// </returns>
        public static JObject ToJson(this Iteration iteration)
        {
            Guard.AssertNotNull(iteration, "iteration");

            var json = ((StoryCardCollection) iteration).ToJson();

            json.AddProperty(Agile.Constants.Properties.Iteration.AvailableEffort,
                             iteration.AvailableEffort.ToString(
                                 Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Iteration.EndDate,
                             iteration.EndDate.ToString("o", CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Iteration.StartDate,
                             iteration.StartDate.ToString("o", CultureInfo.InvariantCulture));

            iteration.View.ToJson(json);
            iteration.Tracking.ToJson(json);

            return json;
        }

        /// <summary>
        /// Converts a <see cref="Backlog"/> to its JSON serialized representation
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        /// <returns>
        /// The JSON serialized representation of the <see cref="Backlog"/>
        /// </returns>
        public static JObject ToJson(this Backlog backlog)
        {
            Guard.AssertNotNull(backlog, "backlog");

            var json = ((StoryCardCollection) backlog).ToJson();

            return json;
        }

        /// <summary>
        /// Converts a <see cref="Project"/> to its JSON serialized representation
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        /// The JSON serialized representation of the <see cref="Project"/>
        /// </returns>
        public static JObject ToJson(this Project project)
        {
            Guard.AssertNotNull(project, "project");

            var json = ((PlanningArtefact) project).ToJson();

            if (project.Legend != null &&
                project.Legend.Count > 0)
            {
                var legendElement = new JObject();

                foreach (Mapping mapping in project.Legend)
                {
                    var property = new JProperty(mapping.Name, Common.Convert.ToHex(mapping.Color));
                    legendElement.Add(property);
                }

                json.AddProperty(Agile.Constants.Properties.Project.Legend, legendElement);
            }

            if (project.Backlog != null)
            {
                var jsonBacklog = project.Backlog.ToJson();
                json.AddProperty("Backlog", jsonBacklog);
            }

            if (project.Iterations != null &&
                project.Iterations.Count() > 0)
            {
                var iterationsArray = new JArray();

                foreach (Iteration iteration in project.Iterations)
                {
                    var jsonIteration = iteration.ToJson();
                    iterationsArray.Add(jsonIteration);
                }

                json.AddProperty(Agile.Constants.Properties.Project.Iterations, iterationsArray);
            }

            return json;
        }

        /// <summary>
        /// Adds the values of a <see cref="Trackable"/> to a JSON serialized representation
        /// of an object
        /// </summary>
        /// <param name="trackable">The trackable.</param>
        /// <param name="json">The json.</param>
        public static void ToJson(this Trackable trackable, JObject json)
        {
            Guard.AssertNotNull(trackable, "trackable");
            Guard.AssertNotNull(json, "json");

            json.AddProperty(Agile.Constants.Properties.Trackable.WorstCaseEstimate,
                             trackable.WorstCaseEstimate.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Trackable.Priority,
                             TypeHelper.ToNullSafeString(trackable.Priority))
                .AddProperty(Agile.Constants.Properties.Trackable.Status, TypeHelper.ToNullSafeString(trackable.Status))
                .AddProperty(Agile.Constants.Properties.Trackable.MostLikelyEstimate,
                             trackable.MostLikelyEstimate.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Trackable.ActualEffort,
                             trackable.ActualEffort.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Trackable.BestCaseEstimate,
                             trackable.BestCaseEstimate.ToString(Constants.Precision, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds the values of a <see cref="Visualizable"/> to a JSON serialized representation
        /// of an object
        /// </summary>
        /// <param name="visualizable">The visualizable.</param>
        /// <param name="json">The json.</param>
        public static void ToJson(this Visualizable visualizable, JObject json)
        {
            Guard.AssertNotNull(visualizable, "visualizable");
            Guard.AssertNotNull(json, "json");

            json.AddProperty(Agile.Constants.Properties.Visualizable.Color, Common.Convert.ToHex(visualizable.Color))
                .AddProperty(Agile.Constants.Properties.Visualizable.Height,
                             visualizable.Height.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Visualizable.RotationAngle,
                             visualizable.RotationAngle.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Visualizable.Width,
                             visualizable.Width.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Visualizable.X,
                             visualizable.X.ToString(Constants.Precision, CultureInfo.InvariantCulture))
                .AddProperty(Agile.Constants.Properties.Visualizable.Y,
                             visualizable.Y.ToString(Constants.Precision, CultureInfo.InvariantCulture));
        }

        public static void ToJson(this Legend legend, JObject json)
        {
            Guard.AssertNotNull(legend, "legend");
            Guard.AssertNotNull(json, "json");

            if (legend.Count > 0)
            {
                JArray array = new JArray();

                foreach (Mapping mapping in legend)
                {
                    JObject map = new JObject(
                        new JProperty(Agile.Constants.Properties.Mapping.Name, mapping.Name),
                        new JProperty(Agile.Constants.Properties.Mapping.Color,
                                      Common.Convert.ToHex(mapping.Color)));
                    array.Add(map);
                }

                json.AddProperty(Agile.Constants.Properties.Project.Legend, array);
            }
        }

        #endregion Conversion to JSON
    }
}