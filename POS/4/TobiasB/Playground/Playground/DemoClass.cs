using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    /// <summary>
    /// DemoClass
    /// </summary>
    class DemoClass
    {
        /// <summary>
        /// Start int
        /// </summary>
        private int start;

        /// <summary>
        /// string1 String
        /// </summary>
        protected String string1;

        /// <summary>
        /// pubint int
        /// </summary>
        public int pubint;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="start"> Start int</param>
        /// <param name="string1"> string1 String</param>
        /// <param name="pubint"> pubint int</param>
        public DemoClass(int start, String string1, int pubint)
        {
            this.start = start;
            this.string1 = string1;
            this.pubint = pubint;
        }

        /// <summary>
        /// getpubint
        /// </summary>
        /// <returns>pubint</returns>
        public int getPubInt()
        {
            return pubint;
        }

        /// <summary>
        /// get start
        /// </summary>
        /// <returns>start</returns>
        public int getStart() 
        {
            return start; 
        }

    }
}
