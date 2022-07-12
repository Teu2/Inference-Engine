using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine 
{
    class TruthTable 
    {
        private KnowledgeBase _KB;
        private string _q;
        private List<string> _output;
        private List<string> _kb;
        private int _count;

        public TruthTable(KnowledgeBase KB)
        {
            _KB = KB;
            _q = KB.Ask;
            _output = new List<string>();
            _kb = new List<string>();
            _count = 0;

            string answer = string.Empty;
            StartTruthTable(answer, _KB, _q);
        }

        private void StartTruthTable(string answer, KnowledgeBase _KB, string _q)
        {
            if (TT_ENTAILS(_KB, _q)) // unnecessary parameter passing but i'm just following the lecture slides just in case
            {
                Console.WriteLine($"\n[TT] YES: {_count}");
            }
            else
            {
                Console.WriteLine("\n[TT] NO");
            }
        }

        private bool TT_ENTAILS(KnowledgeBase _KB, string a) // method goes through the truth table
        {
            // if any row where the KB is true and alpha is false - return false
            // if no smoking gun is found in all the rows, returns true

            List<string> symbols = new List<string>(); // a list of proposition symbols in KB and alpha
            Dictionary<string, bool> model = new Dictionary<string, bool>(); // initially an empty model but will be constructed in the TT_CHECK_ALL method

            symbols.Add(a);

            foreach (var c in _KB.Clause) // adding symbols to the list from KB clauses
            {
                if (c.Body != null)
                {
                    foreach (var b in c.Body)
                    {
                        if (!symbols.Contains(b))
                        {
                            //Console.WriteLine($"B:[{b}]");
                            //_kb.Add(b);
                            symbols.Add(b);
                        }
                    }
                }
                if (!symbols.Contains(c.Head))
                {
                    //Console.WriteLine($"H:[{c.Head}]");
                    symbols.Add(c.Head);
                }
            }
            foreach (var c in _KB.SingleClauses)
            {
                if (!symbols.Contains(c))
                {
                    //Console.WriteLine($"H:[{c}]");
                    //_kb.Add(c);
                    symbols.Add(c);
                }
            }

            _kb = new List<string>(symbols);

            return TT_CHECK_ALL(_KB, a, symbols, model); // model parameter is used for mapping symbols with true or false values
        }

        private bool TT_CHECK_ALL(KnowledgeBase KB, string a, List<string> symbols, Dictionary<string, bool> model) // constructs models starting from the empty model
        {
            // each time TT_CHECK_ALL calls itself, a symbol is removed from 'List<string> symbols' and is assigned value in model
            // overall, symbols is the list of symbols that still don't have a value in model

            string p = string.Empty;
            List<string> rest = new List<string>();

            if (KB.SingleClauses.Count == 0) // fixed bug where there is no clause
            {
                return false;
            }

            if (symbols.Count == 0)
            {
                if (PL_TRUE(KB, model))
                {
                    //return PL_TRUE_APLHA(a, model);
                    
                    if (PL_TRUE_APLHA(a, model))
                    {
                        _count++;
                        return PL_TRUE_APLHA(a, model);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                p = symbols[0];
                symbols.Remove(p);

                rest = symbols; // set to equal the rest of symbols except for p

                return TT_CHECK_ALL(KB, a, rest, EXTEND(p, true, model)) && TT_CHECK_ALL(KB, a, rest, EXTEND(p, false, model)); // assigns each possible combination of true and false for each symbol
            }
        }

        private Dictionary<string, bool> EXTEND(string P, bool TrueOrFalse, Dictionary<string, bool> model) // EXTEND() passes and extended model that includes the assignments of the prevous model + new assignment
        {
            Dictionary<string, bool> modelExtend = new Dictionary<string, bool>(model); // model is an association map
            modelExtend.Add(P, TrueOrFalse);

            return modelExtend;
        }

        private bool PL_TRUE_APLHA(string a, Dictionary<string, bool> model)
        {
            bool result = false;

            if (model.ContainsKey(a))
            {
                result = model[a];
            }

            return result;
        }

        private bool PL_TRUE(KnowledgeBase KB, Dictionary<string, bool> model)
        {
            bool result = true;

            foreach (var c in KB.Clause) 
            {
                if (c.Body != null)
                {
                    foreach (var cb in c.Body)
                    {
                        result = result && PL_TRUE_APLHA(cb, model); // model represents a row in the table
                    }
                }
            }

            return result;
        }
    }
}
