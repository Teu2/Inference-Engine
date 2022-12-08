# Inference-Engine
An inference engine made in C# for propositional logic using the following algorithms:
* Truth Table (TT) checking
* Backward Chaining (BC)
* Forward Chaining (FC) algorithms

# implementation
The following algorithms were written using the following pseudocode below

**Truth Table Checking**
```
function TT-ENTAILS?(KB,α) returns true or false
  inputs: KB, the knowledge base, a sentence in propositional logic α, the query, a sentence in propositional logic
  symbols ← a list of the proposition symbols in KB and α
  return TT-CHECK-ALL(KB,α, symbols, { })
  
function TT-CHECK-ALL(KB,α, symbols, model) returns true or false
  if EMPTY?(symbols) then
    if PL-TRUE?(KB, model) then return PL-TRUE?(α, model)
    else return true // when KB is false, always return true
  else do
    P ← FIRST(symbols)
    rest ← REST(symbols)
    return (TT-CHECK-ALL(KB,α, rest, model ∪ {P = true})
      and
      TT-CHECK-ALL(KB,α, rest, model ∪ {P = false }))
```

**Forward Chaining**
```
function PL-FC-ENTAILS?(KB,q) returns true or false
  local variables: count, a table, indexed by a clause, initially the number of premises
                   inferred, a table, indexed by a symbol, each entry is initially false
                   agenda, a list of symbols, initially the symbols known to be true
  while agenda is not empty do
    p ← POP(agenda)
    unless iferred[p] do
      inferred[p] ← true
      for each Horn clause c in whose premise p appears do
        decrement count[c]
        if count[c] = 0 then do
          if Head[c] = q then return true
          PUSH[HEAD[c], agenda]
  return false
```
