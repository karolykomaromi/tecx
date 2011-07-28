using System.Linq;

using Ploeh.AutoFixture;

namespace TecX.Agile.Test
{
    public static class TestHelper
    {
        public static Fixture GetCustomizedFixture()
        {
            Fixture fixture = new Fixture();

            fixture.Customize<Visualizable>(ob => ob.With(vis => vis.Color,
                                                          fixture.CreateMany<byte>(4).ToArray()));

            fixture.Customize<Mapping>(
                ob =>
                ob.FromFactory(
                    () => new Mapping(fixture.CreateAnonymous<string>(), fixture.CreateMany<byte>(4).ToArray())));

            fixture.Customize<Legend>(ob => ob.Do(lgnd =>
                                                      {
                                                          var mappings = fixture.CreateMany<Mapping>();
                                                          foreach (Mapping mapping in mappings)
                                                          {
                                                              lgnd.Add(mapping.Name, mapping.Color);
                                                          }
                                                      }));

            fixture.Customize<Iteration>(ob => ob.Do(iter =>
                                                         {
                                                             var cards = fixture.CreateMany<StoryCard>();
                                                             foreach (StoryCard storyCard in cards)
                                                             {
                                                                 iter.Add(storyCard);
                                                             }
                                                         }));

            fixture.Customize<Backlog>(ob => ob.Do(blg =>
                                                       {
                                                           var cards = fixture.CreateMany<StoryCard>();
                                                           foreach (StoryCard storyCard in cards)
                                                           {
                                                               blg.Add(storyCard);
                                                           }
                                                       }));

            fixture.Customize<Project>(ob => ob.Do(prj =>
                                                       {
                                                           var iterations = fixture.CreateMany<Iteration>();
                                                           foreach (Iteration iteration in iterations)
                                                           {
                                                               prj.Add(iteration);
                                                           }
                                                       }));

            return fixture;
        }
    }
}
