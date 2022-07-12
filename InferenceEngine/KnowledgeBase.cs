using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class KnowledgeBase
    {
        private List<HornClause> _clause;
        private string _displayFullClause;
        private List<string> _singleClause; // these are known literals e.g a; b; p2; etc..
        private string _ask;

        public KnowledgeBase()
        {
            _clause = new List<HornClause>();
            _singleClause = new List<string>();
        }

        public void GetKnowledgeBase(string fileName)
        {
            StreamReader reader = File.OpenText(fileName);

            if (File.Exists(fileName))
            {
                string clause = string.Empty;
                int i = 0;

                while ((clause = reader.ReadLine()) != null)
                {
                    i++;
                    if (i == 2)
                    {
                        _displayFullClause = clause;
                        clause = clause.Replace(" ", String.Empty); // removes white spaces in the knowledge base

                        string[] clauseSplit = clause.Split(';');
                        int count = 0;

                        foreach (var c in clauseSplit)
                        {
                            count++;
                            if (count == clauseSplit.Length)
                            {
                                break;
                            }

                            if (c.Contains("=>"))
                            {
                                List<string> bodytrim = new List<string>();

                                int index = c.IndexOf("=>");
                                string body = c.Substring(0, index);
                                string head = c.Substring(index + 2); // removes '=>' trail

                                head = head.Trim();
                                string[] splitbody = { "" };

                                //Console.WriteLine($"{body}:{head}");

                                if (body.Contains("&"))
                                {
                                    //string[] splitAnd = { "" };
                                    //splitAnd = body.Split('&');
                                    //string join = splitAnd[0] + " " + splitAnd[1];
                                    //splitbody[0] = join;
                                    splitbody = body.Split('&');
                                }
                                else // clauses without an '&'
                                {
                                    splitbody[0] = body;
                                }
                                foreach (var split in splitbody)
                                {
                                    bodytrim.Add(split);
                                }
                                _clause.Add(new HornClause(bodytrim, head));
                            }
                            else
                            {
                                _singleClause.Add(c);
                                //Console.WriteLine(c);
                            }
                        }
                    }
                    if (i == 4)
                    {
                        _ask = clause; // not actually a clause just the query
                    }
                }
            }
            else
            {
                Console.WriteLine("file doesn't exist");
            }
        }

        public List<HornClause> Clause
        {
            get { return _clause; }
        }

        public List<string> SingleClauses
        {
            get { return _singleClause; }
        }

        public string DisplayFullClause
        {
            get { return _displayFullClause; }
        }

        public string Ask
        {
            get { return _ask; }
            set { _ask = value; }
        }
    }
}




