using System;
using System.Collections.Generic;
using System.Linq;

namespace hackQ1
{

    public class Item
    {
        public int id { get; set; }
        public int weight { get; set; }
        public int width { get; set; }
    }
    class Program
    {
        public static bool[,] dp;
        public static List<List<int>>[] finalVals;
        public static int[] vals;
        public static int[] weights;
        public static int[] ids;
        public static int matchedWidth;

        public static int curMinId;

        static void Main(string[] args)
        {
            bool subsetNotFound = true;
            bool finished = false;
            finalVals = new List<List<int>>[3];
            finalVals[0] = new List<List<int>>();
            finalVals[1] = new List<List<int>>();
            finalVals[2] = new List<List<int>>();


            List<string> input = new List<string>();

            // first read input till there are nonempty items, means they are not null and not ""
            // also add read item to list do not need to read it again    
            string line;
            List<int> tempVals = new List<int>();
            List<int> tempWeights = new List<int>();
            List<int> tempIds = new List<int>();
            while ((line = Console.ReadLine()) != null && line != "")
            {
                string[] splittedLine = line.Split(' ');
                tempIds.Add(Int32.Parse(splittedLine[0]));
                tempVals.Add(Int32.Parse(splittedLine[1]));
                tempWeights.Add(Int32.Parse(splittedLine[2]));
            }

            vals = tempVals.ToArray();
            ids = tempIds.ToArray();
            weights = tempWeights.ToArray();
            curMinId = ids.Count();
            // vals = new int[] { 400, 500, 500, 200, 400, 950 };
            // weights = new int[] { 200, 300, 300, 200, 200, 800 };
            //ids = new int[] { 1, 2, 3, 4, 5, 6 };

            if (vals.Length > 0)
            {
                int n = vals.Length;
                int sum = 1100;
                findSubset(vals, n, sum, 0);
                if (finalVals[0].Count == 0)
                {
                    int lastMatched = 0;
                    while (subsetNotFound)
                    {
                        if (lastMatched != matchedWidth)
                        {
                            lastMatched = matchedWidth;
                            findSubset(vals, n, matchedWidth - 1, 0);
                            if (finalVals[0].Count != 0)
                            {
                                subsetNotFound = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("A:");
                            Console.WriteLine("B:");
                            Console.WriteLine("C:");
                            finished = true;
                            break;
                        }
                    }
                }

                subsetNotFound = true;
                var selectedIndexes = selectForBasket(finalVals[0]);
                sortInsideBasket(selectedIndexes, 'A');
                removeUsedItems(selectedIndexes);
            }
            else
            {
                Console.WriteLine("A:");
                Console.WriteLine("B:");
                Console.WriteLine("C:");
            }
            //-----------------------------------------------------------------------------

            if (vals.Length > 0 && !finished)
            {
                int n = vals.Length;
                findSubset(vals, n, matchedWidth, 1);
                if (finalVals[1].Count == 0)
                {
                    int lastMatched = 0;
                    while (subsetNotFound)
                    {
                        if (lastMatched != matchedWidth)
                        {
                            lastMatched = matchedWidth;
                            findSubset(vals, n, matchedWidth - 1, 1);
                            if (finalVals[1].Count != 0)
                            {
                                subsetNotFound = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("B:");
                            Console.WriteLine("C:");
                            finished = true;
                            break;
                        }
                    }
                }

                subsetNotFound = true;
                var selectedIndexes = selectForBasket(finalVals[1]);
                sortInsideBasket(selectedIndexes, 'B');
                removeUsedItems(selectedIndexes);
            }
            else
            {
                Console.WriteLine("B:");
                Console.WriteLine("C:");
            }
            //-----------------------------------------------------------------------------------
            if (vals.Length > 0 && !finished)
            {
                int n = vals.Length;
                findSubset(vals, n, matchedWidth, 2);
                if (finalVals[1].Count == 0)
                {
                    int lastMatched = 0;

                    while (subsetNotFound)
                    {
                        if (lastMatched != matchedWidth)
                        {
                            lastMatched = matchedWidth;
                            findSubset(vals, n, matchedWidth - 1, 2);
                            if (finalVals[1].Count != 0)
                            {
                                subsetNotFound = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("C:");
                            finished = true;
                            break;
                        }

                    }
                }

                subsetNotFound = true;
                var selectedIndexes = selectForBasket(finalVals[2]);
                sortInsideBasket(selectedIndexes, 'C');
            }
            else
            {
                Console.WriteLine("C:");
            }

            Console.ReadLine();
        }

        public static void removeUsedItems(List<int> usedIndexes)
        {
            List<int> lIds = new List<int>();
            List<int> lVals = new List<int>();
            List<int> lWeights = new List<int>();
            for (int i = 0; i < vals.Length; i++)
            {
                if (!usedIndexes.Contains(i))
                {
                    lIds.Add(ids[i]);
                    lVals.Add(vals[i]);
                    lWeights.Add(weights[i]);
                }
            }
            vals = lVals.ToArray();
            ids = lIds.ToArray();
            weights = lWeights.ToArray();
        }

        public static void sortInsideBasket(List<int> subset, char c)
        {
            //sort by width
            List<Item> items = new List<Item>();
            foreach (var item in subset)
            {
                items.Add(new Item()
                {
                    width = vals[item],
                    id = ids[item],
                    weight = weights[item]
                });
            }

            items = items.OrderByDescending(o => o.width).ThenByDescending(o => o.weight).ThenBy(o => o.id).ToList();
            Console.Write(c.ToString() + ":");
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write(items[i].id.ToString());
                if (i < items.Count - 1)
                {
                    Console.Write(",");
                }
            }
            Console.WriteLine();
        }

        public static List<int> selectForBasket(List<List<int>> combinations)
        {
            if (combinations.Count == 1)
            {
                return combinations[0];
            }
            else
            {
                //consider weight
                int maxWeight = 0;
                List<List<int>> sameWeights = new List<List<int>>();
                foreach (var item in combinations)
                {
                    int curWeight = 0;
                    foreach (var item2 in item)
                    {
                        curWeight += weights[item2];
                    }
                    if (curWeight > maxWeight)
                    {
                        maxWeight = curWeight;
                        sameWeights.Clear();
                    }
                    if (curWeight == maxWeight)
                    {
                        sameWeights.Add(item);
                    }
                }

                if (sameWeights.Count == 1)
                {
                    return sameWeights[0];
                }
                else
                {
                    //consider no of items
                    int maxNoOfItems = 0;
                    List<List<int>> sameNoOfItems = new List<List<int>>();
                    foreach (var item in sameWeights)
                    {
                        int curNoOfItems = item.Count;

                        if (curNoOfItems > maxNoOfItems)
                        {
                            maxNoOfItems = curNoOfItems;
                            sameNoOfItems.Clear();
                        }
                        if (curNoOfItems == maxNoOfItems)
                        {
                            sameNoOfItems.Add(item);
                        }
                    }
                    if (sameNoOfItems.Count == 1)
                    {
                        return sameNoOfItems[0];
                    }
                    else
                    {
                        //consider lowest ids

                        List<List<int>> itemToReturn = new List<List<int>>();

                        var count = sameNoOfItems[0].Count - 1;
                        var noOfSets = sameNoOfItems.Count - 1;
                        var minId = curMinId;

                        for (int i = count; i >= 0; i--)
                        {
                            for (int j = noOfSets; j >= 0; j--)
                            {
                                if (sameNoOfItems[j][i] < curMinId)
                                {
                                    curMinId = sameNoOfItems[j][i];
                                    itemToReturn.Clear();


                                }
                                if (sameNoOfItems[j][i] == curMinId)
                                {
                                    itemToReturn.Add(sameNoOfItems[j]);
                                }

                            }
                            if (itemToReturn.Count == 1)
                            {
                                return itemToReturn[0];
                            }
                            curMinId = minId;
                            sameNoOfItems = itemToReturn.ToList();
                            itemToReturn.Clear();
                            noOfSets = sameNoOfItems.Count - 1;
                        }

                        return itemToReturn[0];
                    }
                }
            }
        }

        static void display(List<int> v, int basket)
        {
            List<int> itemset = new List<int>();
            int totalWeight = 0;
            foreach (var item in v)
            {
                itemset.Add(item);
                totalWeight += weights[item];
            }
            if (totalWeight <= 1000)
            {
                finalVals[basket].Add(itemset);
            }
        }

        public static void printSubsetsRec(int[] arr, int i, int sum,
                                       List<int> p, int basket)
        {
            // If we reached end and sum is non-zero. We print
            // p[] only if arr[0] is equal to sun OR dp[0][sum]
            // is true.
            if (i == 0 && sum != 0 && dp[0, sum])
            {
                p.Add(i);
                display(p, basket);
                p.Clear();
                return;
            }

            // If sum becomes 0
            if (i == 0 && sum == 0)
            {
                display(p, basket);
                p.Clear();
                return;
            }

            // If given sum can be achieved after ignoring
            // current element.
            if (dp[i - 1, sum])
            {
                // Create a new vector to store path
                List<int> b = new List<int>();
                foreach (var item in p)
                {
                    b.Add(item);
                }
                printSubsetsRec(arr, i - 1, sum, b, basket);
            }

            // If given sum can be achieved after considering
            // current element.
            if (sum >= arr[i] && dp[i - 1, sum - arr[i]])
            {
                p.Add(i);
                printSubsetsRec(arr, i - 1, sum - arr[i], p, basket);
            }
        }

        public static void findSubset(int[] arr, int n, int sum, int basket)
        {
            if (n == 0 || sum < 0)
                return;

            dp = new bool[n, sum + 1];
            for (int i = 0; i < n; ++i)
            {
                dp[i, 0] = true;
            }

            if (arr[0] <= sum)
                dp[0, arr[0]] = true;

            for (int i = 1; i < n; ++i)
                for (int j = 0; j < sum + 1; ++j)
                    dp[i, j] = (arr[i] <= j) ? (dp[i - 1, j] ||
                                               dp[i - 1, j - arr[i]])
                                             : dp[i - 1, j];

            for (int i = sum; i >= 0; i--)
            {
                if (dp[n - 1, i])
                {
                    matchedWidth = i;
                    List<int> p = new List<int>();
                    printSubsetsRec(arr, n - 1, i, p, basket);
                    break;
                }
            }

        }
    }
}