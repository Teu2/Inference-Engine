using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ForwardChaining 
    {
        private KnowledgeBase _KB;
        private string _q;
        private List<string> _output;

        public ForwardChaining(KnowledgeBase KB)
        {
            _KB = KB;
            _q = KB.Ask;
            _output = new List<string>();

            string answer = string.Empty;
            StartForwardChaining(answer, _KB, _q);
        }

        private void StartForwardChaining(string answer, KnowledgeBase _KB, string _q)
        {
            if (PL_FC_ENTAILS(_KB, _q)) // parameters for FC algorithm slide 27
            {
                for (int i = 0; i < _output.Count; i++)
                {
                    answer += _output[i];

                    if (i < _output.Count - 1)
                    {
                        answer += ", ";
                    }
                }
                Console.WriteLine($"[FC] YES: {answer}");
            }
            else
            {
                Console.WriteLine("[FC] NO");
            }
        }

        private bool PL_FC_ENTAILS(KnowledgeBase _KB, string _q)
        {
            // local variables, dictionary is used to avoid duplicate keys
            Dictionary<HornClause, int> count = new Dictionary<HornClause, int>(); // table indexed by clauses
            Dictionary<string, bool> inferred = new Dictionary<string, bool>(); // table indexed by symbol
            List<string> agenda = new List<string>(); // a list of symbols
            
            string p = string.Empty;

            foreach (var c in _KB.Clause)
            {
                if (c.Body != null)
                {
                    count[c] = c.Body.Count(); // adds the initial number of premises
                }
            }

            inferred = inferred.ToDictionary(s => s.Key, s => false); // sets all values initially to false

            foreach (var c in _KB.SingleClauses)
            {
                agenda.Add(c); // adds the symbols in the knowledge base known to be true i.e ones without a body
            }

            // COS30019_Lecture_07_2spp slide 27 - algorithm
            while (agenda.Count != 0) // first check the symbols we know are already true, symbols without a body
            {
                p = agenda[0];
                _output.Add(p);

                agenda.RemoveAt(0);

                if (p.Equals(_q)) 
                {
                    return true;
                }

                bool key = inferred.ContainsKey(p); // key is for checking if inferred contains the symbol 'p' and the value is false

                if (inferred.ContainsKey(p).Equals(false)) // unless inferred
                {
                    inferred[p] = true;

                    foreach (var c in _KB.Clause)
                    {
                        if (c.Body != null)
                        {
                            if (c.Body.Contains(p)) 
                            {
                                count[c]--; // decrement count[c]--

                                if (count[c] == 0)
                                {
                                    agenda.Add(c.Head);
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
