using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebXuxeria.Helpers
{
    public class Helpers
    {
        //public static string Label(string target, string text)
        //{
        //    return String.Format("<label for='{0}'>{1}</label>", target, text);
        //}

        public static HtmlString RndCss(Random rnd)
        {
            //ROTATION
            var ro = (rnd.NextDouble() ).ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
            var sign = (rnd.NextDouble() > 0.5) ? "-" : "";
            var result = " style='transform:rotate(" + sign + ro + "deg) ";

            //SCALE
            ro = ((rnd.NextDouble() / 15) + 0.90).ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
            sign = (rnd.NextDouble() > 0.5) ? "-" : "";
            result += "scale("+ ro+ ") ' " ;
            
            return new HtmlString(result);
        }

        public static HtmlString AllPartsRandomized(Word word, bool randomize=true, bool toUpper = true)
        {
            Random rnd = new Random();

            var list = new List<int> { 1, 2, 3, 4 };
            if (randomize)
                list = list.OrderBy(item => rnd.Next()).ToList();

            var result = "";
            result += IndexWord(word, list[0], rnd, toUpper);
            result += IndexWord(word, list[1], rnd, toUpper);
            result += IndexWord(word, list[2], rnd, toUpper);
            result += IndexWord(word, list[3], rnd, toUpper);

            return new HtmlString(result);
        }
        public static HtmlString IndexWord(Word word, int partid, Random rnd, bool toUpper)
        {
            
            var result  = "<li class='ui-state-default wood button' data-partid='" +  partid + "'  onclick='playOwnAndRestore(this)' ontouchstart='playOwnAndRestore(this)'  " +  RndCss(rnd) + ">";
            result += " <audio src='/Sound?wordid=" + word.WordId + "&partid=" + partid + "'  preload='auto'></audio>";

            var part = "";
            switch (partid)
            {
                case 1:
                    part = word.Part1;
                    break;
                case 2:
                    part = word.Part2;
                    break;
                case 3:
                    part = word.Part3;
                    break;
                case 4:
                    part = word.Part4;
                    break;
            }

            if (string.IsNullOrEmpty(part))
                return new HtmlString("");

            if (toUpper)
                part = part.ToUpper();

            result += part;
            result += " </li>";

            return new HtmlString(result);
        }
    }
}
