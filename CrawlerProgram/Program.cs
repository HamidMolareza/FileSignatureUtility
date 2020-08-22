using System;
using FunctionalUtility.Extensions;

namespace CrawlerProgram {
    public static class Program {
        public static void Main () =>
            TryExtensions.Try (Crawler.Start)
            .Tee (methodResult =>
                Console.WriteLine (methodResult.IsSuccess ? "Success" : methodResult.Detail.ToString ()));
    }
}