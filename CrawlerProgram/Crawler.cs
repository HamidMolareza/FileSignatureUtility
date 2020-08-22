using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalUtility.Extensions;
using HtmlAgilityPack;

namespace CrawlerProgram
{
     public static class Crawler
    {
        public static void Start()
        {
            const string baseUrl = "https://www.filesignatures.net/index.php?page=all&order=EXT&alpha=&currentpage=";
            const int numOfPage = 18;
            const int approximateItemsPerPage = 30;
            
            var web = new HtmlWeb();
            var allTypeData = new List<TypeData>(approximateItemsPerPage * numOfPage);
            for (var i = 1; i <= numOfPage; i++)
            {
                var targetUrl = baseUrl + i;
                var doc = web.Load(targetUrl);
                var currentPageData = doc.DocumentNode
                    .SelectNodes("//table/tr/td")
                    .Select(node => node.InnerText)
                    .Skip(6)
                    .Where(data => data != "\n\n            ")
                    .ToList()
                    .Map(MapToModel);
                allTypeData.AddRange(currentPageData);
            }
            
            var uniqueData = allTypeData
                .GroupBy(typeData => typeData.TypeName)
                .ToList()
                .Map(MergeGroups);
            
            //TODO: What to do?
        }

        private static List<TypeData> MapToModel(List<string> data)
        {
            if (data.Count % 3 != 0)
                throw new Exception();

            var result = new List<TypeData>(data.Count / 3);
            for (var i = 0; i < data.Count - 3; i += 3)
            {
                result.Add(new TypeData(
                    data[i], data[i + 1], data[i + 2]));
            }

            return result;
        }

        private static List<TypeData> MergeGroups(IReadOnlyCollection<IGrouping<string, TypeData>> groups) =>
            new List<TypeData>(groups.Count)
                .Tee(list => list.AddRange(groups.Select(MergeGroup)));

        private static TypeData MergeGroup(IGrouping<string, TypeData> group)
        {
            var result = new TypeData(group.Key, group.First().Description);
            foreach (var typeData in group)
                result.Signatures.AddRange(typeData.Signatures);
            return result;
        }
    }
}