using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    public abstract class MinimumQualityRecipe<T> : Recipe
        where T : Item
    {
        protected class Combination
        {
            public List<T> Match;
            public int Total;
            public bool Perfect;
        }

        protected List<Combination> combinations;
        protected bool stop;
        protected int REQUIREDQUALITY = 40;
        protected const decimal DEFAULT_MINIMUM_MATCH_PERCENT = 70;

        public MinimumQualityRecipe()
            : base(DEFAULT_MINIMUM_MATCH_PERCENT)
        { }

        public MinimumQualityRecipe(decimal returnMatchesGreaterThan)
            : base (returnMatchesGreaterThan)
        { }

        protected abstract IEnumerable<T> getCandidateItems(IEnumerable<Item> items);

        protected virtual string getMissingCombinationText(decimal requiredQuality, decimal qualityFound)
        {
            return string.Format("Item(s) with quality totaling {0}%", requiredQuality - qualityFound);
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<POEApi.Model.Item> items)
        {
            List<T> candidateItems = getCandidateItems(items).ToList();

            bool canContinue = true;
            while (canContinue)
            {
                getCombinations(candidateItems, REQUIREDQUALITY);

                Combination perfect = combinations.Find(c => c.Perfect);
                if (perfect != null)
                {
                    perfect.Match.ForEach(g => candidateItems.Remove(g));
                    yield return getResult(perfect);
                    continue;
                }

                Combination leastOver = null;
                List<Combination> over = combinations.FindAll(c => !c.Perfect);
                if (over != null && over.Count > 0)
                    leastOver = over.OrderBy(c => c.Total).First();

                if (leastOver != null)
                {
                    leastOver.Match.ForEach(g => candidateItems.Remove(g));
                    yield return getResult(leastOver);
                    continue;
                }

                if (leastOver == null)
                    canContinue = false;
            }

            Combination remaining = new Combination() { Match = candidateItems, Total = candidateItems.Sum(a => a.Quality), Perfect = false };
            RecipeResult leftOver = getResult(remaining);
            if (leftOver.IsMatch)
                yield return leftOver;
        }

        private void getCombinations(List<T> pool, int target, List<T> workingSet)
        {
            if (stop)
                return;

            int current = 0;
            foreach (T x in workingSet)
            {
                current += x.Quality;
            }
            if (current == target)
            {
                combinations.Add(new Combination() { Match = new List<T>(workingSet), Perfect = true, Total = target });
                stop = true;
                return;
            }
            if (current > target)
            {
                combinations.Add(new Combination() { Match = new List<T>(workingSet), Perfect = false, Total = current });
                return;
            }
            for (int i = 0; i < pool.Count; i++)
            {
                var remaining = new List<T>();
                T n = pool[i];
                for (int j = i + 1; j < pool.Count; j++)
                {
                    remaining.Add(pool[j]);
                }
                var workingInternal = new List<T>(workingSet);
                workingInternal.Add(n);
                getCombinations(remaining, target, workingInternal);
            }
        }
        private void getCombinations(List<T> pool, int target)
        {
            combinations = new List<Combination>();
            stop = false;
            getCombinations(pool, target, new List<T>());
        }

        private RecipeResult getResult(Combination currentSet)
        {
            RecipeResult result = new RecipeResult();
            result.MatchedItems = currentSet.Match.Cast<Item>().ToList();
            result.Instance = this;
            result.IsMatch = true;

            decimal total = currentSet.Total;
            decimal match = (total / REQUIREDQUALITY) * 100;

            result.IsMatch = match >= base.ReturnMatchesGreaterThan;
            result.PercentMatch = match;
            if (match < 100)
                result.Missing = new List<string>() { getMissingCombinationText(REQUIREDQUALITY, total) };

            return result;
        }
    }
}
