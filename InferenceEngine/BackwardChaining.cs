using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class BackwardChaining
    {
        private KnowledgeBase _KB;
        private string _q;
        private List<string> _output;

        public BackwardChaining(KnowledgeBase KB)
        {
            _KB = KB;
            _q = KB.Ask;
            _output = new List<string>();

            string answer = string.Empty;
            StartBackwardChaining(answer, _KB, _q);
        }

        private void StartBackwardChaining(string answer, KnowledgeBase _KB, string _q)
        {
            if (BC_ENTAILS(_KB, _q))
            {
                for (int i = 0; i < _output.Count; i++)
                {
                    answer += _output[i];

                    if (i < _output.Count - 1)
                    {
                        answer += ", ";
                    }
                }
                Console.WriteLine($"[BC] YES: {answer}\n");
            }
            else
            {
                Console.WriteLine("[BC] NO\n");
            }
        }

        private bool BC_ENTAILS(KnowledgeBase _KB, string _q) 
        {
            string p = string.Empty;
            bool result = false;

            List<string> agenda = new List<string>(); // a list of symbols
            agenda.Add(_q);

            while (agenda.Count != 0)
            {
                p = agenda[0];

                if (!_output.Contains(p))
                {
                    _output.Add(p);
                }

                agenda.RemoveAt(0);

                if (_KB.SingleClauses.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var s in _KB.SingleClauses) // checks if q is already known from the symbols that are true
                    {
                        if (_q.Equals(s))
                        {
                            return true;
                        }
                    }
                }

                foreach (var c in _KB.Clause)
                {
                    if (p.Equals(c.Head))
                    {
                        //Console.WriteLine($"_q equals c.head _Q:[{_q}] C.HEAD[{c.Head}]");
                        result = true;

                        foreach (var b in c.Body)
                        {
                            //Console.WriteLine($"B:[{b}]");
                            agenda.Add(b);

                            if (!_output.Contains(b)) // ignores duplicates 
                            {
                                _output.Add(b);
                            }
                        }
                    }
                }
            }
            _output.Reverse(); // reverses the output to be similar to the assignment output

            return result;
        }
    }
}
