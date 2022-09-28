using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordsManager.Utils
{
    internal class ListFilter
    {
        public string Name { get; set; }
        private SortBy SortByClass { get; set; }
        public ListFilter(string name, SortBy sortByClass)
        {
            Name = name;
            SortByClass = sortByClass;
        }
        public override string ToString()
        {
            return Name;
        }
        public List<T> Sort<T>(List<T> listToSort)
        {
            return SortByClass.Sort(listToSort);
        }
    }


    public class SortBy
    {
        string Key { get; set; }
        public SortBy(string key)
        {
            Key = key;
        }
        public List<T> Sort<T>(List<T> listToSort)
        {
            return listToSort.OrderBy(x => Convert.ToString(x.GetType().GetProperty(Key).GetValue(x, null))).ToList();
        }
    }
}
