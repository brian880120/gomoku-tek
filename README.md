# gomoku-tek
TODO:
1) Move the judgement algorithm to the web api service 
2) Adjust when to call the javascript judge method 
3) change columnId and unitId to integer:
{
  columnId: 1,
  unitId: 10
}

Complete:
1) continue play on same side
2) tell UI current user
3) Modify socket object:
{
  Type: ''
  Payload: {}
}
4) Remove reset game / clear game moves 

Pending:
1) add canParticipate attr to login response
Important code refactor need soon:
