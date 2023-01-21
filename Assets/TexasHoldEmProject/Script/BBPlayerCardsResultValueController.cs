using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace BLabTexasHoldEmProject {

public class BBPlayerCardsResultValueController : MonoBehaviour {

	public enum CardsValues {RoyalFlush,StraightFlush,Poker,FullHouse,Flush,Straight,ThreeOfAkind,TwoPair,Pair,HighCard,Null}

	public bool logResult = false;

	public Vector2[] RoyalFlushSimData = {new Vector2(1,10),new Vector2(1,11),new Vector2(1,12),new Vector2(1,13),new Vector2(1,1)}; 
	public Vector2[] StraightFlushSimData = {new Vector2(1,2),new Vector2(1,3),new Vector2(1,4),new Vector2(1,6),new Vector2(1,5)}; 
	public Vector2[] PokerSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,2),new Vector2(1,2)}; 
	public Vector2[] FullHouseSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 
	public Vector2[] FlushSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 
	public Vector2[] StraightSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 
	public Vector2[] ThreeOfAkindSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 
	public Vector2[] TwoPairSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 
	public Vector2[] PairSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 
	public Vector2[] HighCardSimData = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2)}; 

	public Vector2[] RoyalFlushSimDataTurn = {new Vector2(1,10),new Vector2(1,11),new Vector2(1,12),new Vector2(1,13),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] StraightFlushSimDataTurn = {new Vector2(1,2),new Vector2(1,3),new Vector2(1,4),new Vector2(1,6),new Vector2(1,5),new Vector2(1,1)}; 
	public Vector2[] PokerSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,2),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] FullHouseSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] FlushSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] StraightSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] ThreeOfAkindSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] TwoPairSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] PairSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 
	public Vector2[] HighCardSimDataTurn = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1)}; 

	public Vector2[] RoyalFlushSimDataRiver = {new Vector2(1,10),new Vector2(1,11),new Vector2(1,12),new Vector2(1,13),new Vector2(1,1),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] StraightFlushSimDataRiver = {new Vector2(1,2),new Vector2(1,3),new Vector2(1,4),new Vector2(1,6),new Vector2(1,5),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] PokerSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,2),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] FullHouseSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] FlushSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] StraightSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] ThreeOfAkindSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] TwoPairSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] PairSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 
	public Vector2[] HighCardSimDataRiver = {new Vector2(1,2),new Vector2(1,2),new Vector2(1,4),new Vector2(1,4),new Vector2(1,2),new Vector2(1,1),new Vector2(1,1)}; 



	public bool simulateRoyalFlush = false;
	public bool simulateStraightFlush = false;
	public bool simulatePoker = false;
	public bool simulateFullHouse = false;
	public bool simulateFlush = false;
	public bool simulateStraight = false;
	public bool simulateThreeOfAkind = false;
	public bool simulateTwoPair = false;
	public bool simulatePair = false;
	public bool simulateHighCard = false;

	 int chekForHighCard(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList) {
		Vector2[] newCardList = null;

		if(simulateHighCard) {
		    if(openCardsList.Length == 3)
			     newCardList = HighCardSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = HighCardSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = HighCardSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }

//		foreach(Vector2 v2 in newCardList) Debug.Log("chekForHighCard-->newCardList  card : " + v2); 

		int[] valList = new int[newCardList.Length];
		for(int x = 0; x < newCardList.Length;x++) {valList[x] = (int)newCardList[x].y;}

//		foreach(int v in valList) Debug.Log("chekForHighCard-->valList  card : " + v); 

		int CheckForAce = Mathf.Min(valList);

		if(CheckForAce == 1) {
		  return 1;
		}

	    return Mathf.Max(valList);

    }

	 bool chekForPair(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;

		if(simulatePair) {
			if(openCardsList.Length == 3)
			     newCardList = PairSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = PairSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = PairSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }


		int counter = 0;
		bool gotPair = false;


		for(int x = 0; x < newCardList.Length;x++) {

	          for(int y = 0; y < newCardList.Length;y++) {
	             if(newCardList[x].y == newCardList[y].y) {
	               counter++;
	                if(counter == 2) {
	                  maxCardValue = (int)newCardList[x].y;
	                  gotPair = true;
	                  break;
	                }
	             }
	          }
		 if(gotPair) break;
		 counter = 0;
	    }

	    if(gotPair) return true;

	    return false;


	}

	 bool chekForTwoPair(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;

		if(simulateTwoPair) {
			if(openCardsList.Length == 3)
				newCardList = TwoPairSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = TwoPairSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = TwoPairSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }


		int counter = 0;
		bool gotPair = false;
		int[] maxValList = new int[2];

	    for(int x = 0; x < newCardList.Length;x++) {

	          for(int y = 0; y < newCardList.Length;y++) {
	             if(newCardList[x].y == newCardList[y].y) {
	               counter++;
	                if(counter == 2) {
	                  maxValList[0] = (int)newCardList[x].y;
	                  gotPair = true;
	                  break;
	                }
	             }
	          }
		 if(gotPair) break;
		 counter = 0;
	    }

	    if(!gotPair) return false;
	    gotPair = false;
	    counter = 0;

		for(int x = 0; x < newCardList.Length;x++) {
	          for(int y = 0; y < newCardList.Length;y++) {
	             if( (newCardList[x].y == newCardList[y].y) && (newCardList[x].y != maxValList[0]) ) {
	               counter++;
	                if(counter == 2) {
	                  maxValList[1] = (int)newCardList[x].y;
	                  gotPair = true;
	                  break;
	                }
	             }
	          }
		 if(gotPair) break;
		 counter = 0;
	    }

	    if(gotPair) {
	      maxCardValue = Mathf.Max(maxValList);
	      return true;
	    }

	    return false;
	}

	 bool chekForThreeOfAkind(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;

		if(simulateThreeOfAkind) {
			if(openCardsList.Length == 3)
				newCardList = ThreeOfAkindSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = ThreeOfAkindSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = ThreeOfAkindSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }

		int counter = 0;
		bool gotTris = false;

	    for(int x = 0; x < newCardList.Length;x++) {

	          for(int y = 0; y < newCardList.Length;y++) {
	             if(newCardList[x].y == newCardList[y].y) {
	               counter++;
	                if(counter == 3) {
	                  maxCardValue = (int)newCardList[x].y;
	                  gotTris = true;
	                  break;
	                }
	             }
	          }
		 if(gotTris) break;
		 counter = 0;
	    }

	    if(gotTris) {
	        return true;
	    } 

	        return false;
	    
	}

	public bool chekForStraight(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;

		if(simulateStraight) {
			if(openCardsList.Length == 3)
				newCardList = StraightSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = StraightSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = StraightSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }

//		foreach(Vector2 v in newCardList) Debug.Log("newCardList ===============================================newCardList================================================================= newCardList : " + v);

	    List<int> valList = new List<int>();

		for(int x = 0; x < newCardList.Length;x++) { valList.Add( (int)newCardList[x].y );}

		Vector2 resVal;
		int maxSequence = 0;
		int maxVal = 0;

		for(int x = 0; x < newCardList.Length;x++) {

		  resVal = BBStaticData.getSequenceForNumer(valList[x],valList);
		  if(resVal.x > maxSequence) {
		      maxSequence = (int)resVal.x; 
			  maxVal = (int)resVal.y;
		  }

		}

//		Debug.Log("maxSequence : " + maxSequence);

		maxCardValue = maxVal;

		if(maxSequence > 4) return true;


/*
		if(logResult) Debug.Log("chekForStraight@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 3 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@  maxVal : " + maxVal);

		if(gotStraight) {
			maxCardValue = maxVal;
			return true;
		} else {
		   return false;
		}
*/

/*
		bool gotNext = false; minValue++; 

		for(int x = 0; x < newCardList.Length;x++) {
		    if(newCardList[x].y == minValue) {
		      gotNext = true;break;
		    }
		 }

		if(gotNext) {} else {return false;} //2° OK

		gotNext = false; minValue++; for(int x = 0; x < newCardList.Length;x++) {if(newCardList[x].y == minValue) {gotNext = true;break;}}
		if(gotNext) {} else {return false;} //3° OK

		gotNext = false; minValue++; for(int x = 0; x < newCardList.Length;x++) {if(newCardList[x].y == minValue) {gotNext = true;break;}}
		if(gotNext) {} else {return false;} //4° OK

		gotNext = false; minValue++; for(int x = 0; x < newCardList.Length;x++) {if(newCardList[x].y == minValue) {gotNext = true;break;}}
		if(gotNext) {
			maxCardValue = minValue;
			return true;
		} else {return false;} //5° OK

*/
		return false;

	}

	 bool chekForFlush(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;
		int[] maxCardValueList = new int[5];

		if(simulateFlush) {
			if(openCardsList.Length == 3)
				newCardList = FlushSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = FlushSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = FlushSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }

		int checkVal_1 = 1;
		int checkVal_2 = 2;
		int checkVal_3 = 3;
		int checkVal_4 = 4;

	    int counter = 0;
		for(int x = 0; x < newCardList.Length;x++) {if((int)newCardList[x].x == checkVal_1) {maxCardValueList[counter] = (int)newCardList[x].y;counter++;if(counter == 5) {break;}}}
		 maxCardValue = Mathf.Max( maxCardValueList );
        if(counter == 5) {return true;}

		counter = 0;
		for(int x = 0; x < newCardList.Length;x++) {if(newCardList[x].x == checkVal_2) {maxCardValueList[counter] = (int)newCardList[x].y;counter++;if(counter == 5) {break;}}}
		 maxCardValue = Mathf.Max( maxCardValueList );
        if(counter == 5) {return true;}

		counter = 0;
		for(int x = 0; x < newCardList.Length;x++) {if(newCardList[x].x == checkVal_3) {maxCardValueList[counter] = (int)newCardList[x].y;counter++;if(counter == 5) {break;}}}
		 maxCardValue = Mathf.Max( maxCardValueList );
        if(counter == 5) {return true;}

		counter = 0;
		for(int x = 0; x < newCardList.Length;x++) {if(newCardList[x].x == checkVal_4) {maxCardValueList[counter] = (int)newCardList[x].y;counter++;if(counter == 5) {break;}}}
		 maxCardValue = Mathf.Max( maxCardValueList );
        if(counter == 5) {return true;}

        return false;
	}

	 bool chekForFullHouse(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;

		if(simulateFullHouse) {
			if(openCardsList.Length == 3)
				newCardList = FullHouseSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = FullHouseSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = FullHouseSimDataRiver;
	    } else {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
	    }

		int[] valList = new int[newCardList.Length];
		for(int x = 0; x < newCardList.Length;x++) {valList[x] = (int)newCardList[x].y;}

		int counter = 0;
		int valChecked = 0;

		for(int x = 0; x < valList.Length;x++) {
		   for(int y = 0; y < newCardList.Length;y++) {
			 //Debug.Log("FOR 3--> x : " + valList[x] + " y : " + newCardList[y].y + " counter : " + counter);
		     if(valList[x] == (int)newCardList[y].y) {
		       counter++;
		       if(counter == 3) { 
				  //Debug.Log("counter == 3  FOR 3--> x : " + valList[x] + " y : " + newCardList[y].y + " counter : " + counter);
		          valChecked = valList[x];
		          maxCardValue =  valChecked;
		          break;
		       }
		     }    
		   }

		   if(counter == 3) 
		    break;
		   else 
		    counter = 0;
		}

		if(counter == 3) {

		} else {
		 return false;
		}

		counter = 0;
		for(int x = 0; x < valList.Length;x++) {
		   for(int y = 0; y < newCardList.Length;y++) {
			 if( (valList[x] == (int)newCardList[y].y) && (valList[x] != valChecked)) {
		       counter++;
		       if(counter == 2) break;
		     }    
		   }
			if(counter == 2) 
			  break;
            else
              counter = 0;
		}

		if(counter == 2) {
		  return true;
		} else {
		 return false;
		}

	}

	bool chekForPoker(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int maxCardValue) {
		Vector2[] newCardList = null;
		maxCardValue = 0;

		if(simulatePoker) {
			if(openCardsList.Length == 3)
				newCardList = PokerSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = PokerSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = PokerSimDataRiver;
	   } else {
	     try{
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
		 } catch(Exception e) {
		  Debug.Log(e.Message);
		  return false;
		 }
	   }

	   int counter = 0;
	   bool gotPoker = false;

	   for(int x = 0; x < newCardList.Length;x++) {

	      
	          for(int y = 0; y < newCardList.Length;y++) {
	               if(newCardList[x].y == newCardList[y].y) {
	                  counter++;
	                    if(counter == 4) {
	                       gotPoker = true;
	                       maxCardValue = (int)newCardList[x].y;
	                       break;
	                    }
	               }         
	          }
	          if(gotPoker) {
	            break;
	          }
	          counter = 0;
	   }

	   if(gotPoker) return true;

	   return false;

/*
	    int checkVal_1 = (int)newCardList[0].y;
		int checkVal_2 = (int)newCardList[1].y;
		int checkVal_3 = (int)newCardList[2].y;
		int checkVal_4 = (int)newCardList[3].y;

		foreach(Vector2 v2 in newCardList) Debug.Log("chekForPoker--> card : " + v2); 




		int counter = 0;
		for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == checkVal_1) {counter++;seed = (int)newCardList[x].x;}} if(counter == 4) { return true;}
		 
		counter = 0;
		for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == checkVal_2) {counter++;seed = (int)newCardList[x].x;}} if(counter == 4) { return true;}

		counter = 0;
		for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == checkVal_3) {counter++;seed = (int)newCardList[x].x;}} if(counter == 4) { return true;}

		counter = 0;
		for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == checkVal_4) {counter++;seed = (int)newCardList[x].x;}} if(counter == 4) { return true;}

		return false;
*/
	}

	 bool chekForStraightFlush(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int _cardMaxValue) {
		Vector2[] newCardList = null;
		_cardMaxValue = 0;

/*
		    bool got_1 = false;
			bool got_2 = false;
			bool got_3 = false;
			bool got_4 = false;
			bool got_5 = false;
*/

	   if(simulateStraightFlush) {
			if(openCardsList.Length == 3)
				newCardList = StraightFlushSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = StraightFlushSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = StraightFlushSimDataRiver;
	   } else {
	     try{
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
		 } catch(Exception e) {
					Debug.Log(e.Message);
		   return false;
		 }
	   }

//		foreach(Vector2 v2 in newCardList) Debug.Log("##################1#########################################################################chekForStraightFlush--> card : " + v2); 

		int seedCounter = 0;
		int seed = 0;

		for(int j = 1; j < 5;j++) {

			for(int x = 0; x < newCardList.Length;x++) {
                 if(newCardList[x].x == j) {
                   seedCounter++;
                 }			   
			}

			 if(seedCounter > 4) {
			    seed = j;
			    break;
			 }
			 seedCounter = 0;
        }

//		Debug.Log("############2######### seed ######################################################################chekForStraightFlush seed : " + seed);

        if(seed == 0) return false;

		List<int> valList = new List<int>();

		for(int x = 0; x < newCardList.Length;x++) { 
		     if(newCardList[x].x == seed) valList.Add( (int)newCardList[x].y );
		}

		foreach(int v2 in valList) Debug.Log("##########################3 valList #################################################################chekForStraightFlush--> valList : " + v2); 

		Vector2 resVal;
		int maxSequence = 0;
		//int maxVal = 0;

		for(int x = 0; x < valList.Count;x++) {

		  resVal = BBStaticData.getSequenceForNumer(valList[x],valList);
		  if(resVal.x > maxSequence) {
		      maxSequence = (int)resVal.x; 
			  //maxVal = (int)resVal.y;
		  }

		}

		Debug.Log("############4######### maxSequence ######################################################################chekForStraightFlush maxSequence : " + maxSequence);

		if(maxSequence > 4) return true;

        return false;
/*
		int[] cVal = new int[newCardList.Length];
		for(int x = 0; x < newCardList.Length;x++) cVal[x] = (int)newCardList[x].y;
		int lowerVal = Mathf.Min(cVal);

		foreach(Vector2 v2 in newCardList) Debug.Log("###########################################################################################chekForStraightFlush--> card : " + v2 + " min : " + lowerVal); 

		int lastCardSeedToCheck = 0;
		int cardSeedToCheck = 0;

		for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == lowerVal) {got_1 = true;cardSeedToCheck = (int)newCardList[x].x;}}

		lowerVal++;

		if(got_1) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == lowerVal) {got_2 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_2 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

        lowerVal++;

		if(got_2) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == lowerVal) {got_3 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_3 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

		lowerVal++;

		if(got_3) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == lowerVal) {got_4 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_4 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

		lowerVal++;

		if(got_4) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == lowerVal) {_cardMaxValue = (int)newCardList[x].y; got_5 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_5 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

		if(got_5) {
	         return true;
	      }

		return false;
*/
	}

	 bool chekForRoyalFlush(Vector2 playerCard_1,Vector2 playerCard_2,Vector2[] openCardsList, out int seed) {

		Vector2[] newCardList = null;
		seed = 0;

	   if(simulateRoyalFlush) {
			if(openCardsList.Length == 3)
				newCardList = RoyalFlushSimData;
			 else if(openCardsList.Length == 4) 
			     newCardList = RoyalFlushSimDataTurn;
			 else if(openCardsList.Length == 5) 
			     newCardList = RoyalFlushSimDataRiver;
	   } else {
	    try {
			newCardList = new Vector2[openCardsList.Length + 2];
			newCardList[0] = playerCard_1;
			newCardList[1] = playerCard_2;
			for(int x = 0; x < openCardsList.Length;x++) {newCardList[x+2] = openCardsList[x];}
		} catch(Exception e) {
					Debug.Log(e.Message);
		  return false;
		}
	   }


		//foreach(Vector2 v2 in newCardList) Debug.Log("chekForRoyalFlush--> card : " + v2); 

		    bool got_10 = false;
			bool got_11 = false;
			bool got_12 = false;
			bool got_13 = false;
			bool got_1 = false;

		   int cardSeedToCheck = 0;
		   int lastCardSeedToCheck = 0;

		for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == 10) {got_10 = true;cardSeedToCheck = (int)newCardList[x].x;}}

	      if(got_10) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == 11) {got_11 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_11 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

		if(got_11) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == 12) {got_12 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_12 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

		if(got_12) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == 13) {got_13 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_13 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

		if(got_13) {
			for(int x = 0; x < newCardList.Length; x++) {if(newCardList[x].y == 1) {got_1 = true;lastCardSeedToCheck = (int)newCardList[x].x;}}
 			if(got_1 && (lastCardSeedToCheck == cardSeedToCheck) ) {
 			} else {
 			 return false;
 			}
	      } else {
	       return false;
	      }

	      if(got_1) {
			 seed = cardSeedToCheck;
	         return true;
	      }

	      return false;
	}

	//========================================================================//

	public CardsValues getOpenCardsResult(Vector2[] flopCardList, Vector2 turnCard, Vector2 riverCard, out int maxCardValueRet) {
		bool tmpRes = false;
		maxCardValueRet = 0;
		int maxCardValue = 0;

		Vector2[] newCardList = flopCardList;
		 
		tmpRes = chekForRoyalFlush(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForRoyalFlush ---> : " + tmpRes + " : RoyalFlush");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.RoyalFlush; }

		maxCardValue = 0;
		tmpRes = chekForStraightFlush(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForStraightFlush ---> : " + tmpRes + " : StraightFlush");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.StraightFlush; }

		maxCardValue = 0;
		tmpRes = chekForPoker(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForPoker ---> : " + tmpRes + " : Poker");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.Poker; }

		maxCardValue = 0;
		tmpRes = chekForFullHouse(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForFullHouse ---> : " + tmpRes + " : FullHouse");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.FullHouse; }

		maxCardValue = 0;
		tmpRes = chekForFlush(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForFlush ---> : " + tmpRes + " : Flush");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.Flush; }

		maxCardValue = 0;
		tmpRes = chekForStraight(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForStraight ---> : " + tmpRes + " : Straight");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.Straight; }

		maxCardValue = 0;
		tmpRes = chekForThreeOfAkind(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForThreeOfAkind ---> : " + tmpRes + " : ThreeOfAkind");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.ThreeOfAkind; }

		maxCardValue = 0;
		tmpRes = chekForTwoPair(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForTwoPair ---> : " + tmpRes + " : TwoPair");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.TwoPair; }

		maxCardValue = 0;
		tmpRes = chekForPair(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForPair ---> : " + tmpRes + " : Pair");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.Pair; }

		maxCardValue = 0;
		tmpRes = chekForPair(turnCard, riverCard, newCardList, out maxCardValue);
		Debug.Log("*** getOpenCardsResult ***BBPlayerCardsResultValueController*** chekForPair ---> : " + tmpRes + " : Pair");
		if(tmpRes) { maxCardValueRet = maxCardValue; return CardsValues.Pair; }

		int HighCard = chekForHighCard(turnCard, riverCard, newCardList);

		maxCardValueRet = HighCard;

		return CardsValues.HighCard;
	}


	public CardsValues getCardsResultMultiplayer(int playerID,out int maxCardValueRet) {

	//CardsValues CValuesRet = CardsValues.HighCard;

	bool tmpRes = false;
    int retSeed = 0;
	maxCardValueRet = 0;
	int maxCardValue = 0;

	//int cardsListLength = 0;
	Vector2[] newCardList = null;

//		Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% phase : " + GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail);


			if(GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ClosingPreFlop || 
				GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Flop ||
				GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDownAllInOnFlop) {

		        newCardList = new Vector2[3];
				newCardList[0] = GetComponent<BBGameController>().flopCardList[0];
				newCardList[1] = GetComponent<BBGameController>().flopCardList[1];
				newCardList[2] = GetComponent<BBGameController>().flopCardList[2];
		     }
			else if(GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Turn ||
				GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDownAllInOnTurn) {
			    newCardList = new Vector2[4];
				newCardList[0] = GetComponent<BBGameController>().flopCardList[0];
				newCardList[1] = GetComponent<BBGameController>().flopCardList[1];
				newCardList[2] = GetComponent<BBGameController>().flopCardList[2];
				newCardList[3] = GetComponent<BBGameController>().turnCard;
	         }
			else if(GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.River ||
				GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDown ||
				GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDownAllInOnRiver) {
			    newCardList = new Vector2[5];
				newCardList[0] = GetComponent<BBGameController>().flopCardList[0];
				newCardList[1] = GetComponent<BBGameController>().flopCardList[1];
				newCardList[2] = GetComponent<BBGameController>().flopCardList[2];
				newCardList[3] = GetComponent<BBGameController>().turnCard;
				newCardList[4] = GetComponent<BBGameController>().riverCard;
	         }



	if(simulateHighCard) goto TestHighCard;

			tmpRes = chekForRoyalFlush(GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			                   newCardList,out retSeed);
 
		if(logResult) {
		  Debug.Log("***BBPlayerCardsResultValueController*** chekForRoyalFlush ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.RoyalFlush.ToString();
		}

		if(tmpRes) {
			maxCardValueRet = retSeed;
		    return CardsValues.RoyalFlush;
		}


			tmpRes = chekForStraightFlush(GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			              newCardList,out retSeed);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForStraightFlush ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.StraightFlush.ToString() + " : " + retSeed.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = retSeed;
		  return CardsValues.StraightFlush;
		 }

	maxCardValue = 0;
	tmpRes = chekForPoker(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForPoker ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Poker.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Poker;
		 }

	maxCardValue = 0;
	tmpRes = chekForFullHouse(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForFullHouse ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.FullHouse.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.FullHouse;
		 }

     maxCardValue = 0;
	 tmpRes = chekForFlush(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			  newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForFlush ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Flush.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Flush;
		 }

	maxCardValue = 0;
	tmpRes = chekForStraight(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForStraight ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Straight.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Straight;
		 }

	maxCardValue = 0;
	tmpRes = chekForThreeOfAkind(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForThreeOfAkind ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.ThreeOfAkind.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.ThreeOfAkind;
		 }

	maxCardValue = 0;
	tmpRes = chekForTwoPair(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForTwoPair ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.TwoPair.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.TwoPair;
		 }

	maxCardValue = 0;
	tmpRes = chekForPair(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForPair ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Pair.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Pair;
		 }
    
TestHighCard:
        
	int HighCard = GetComponent<BBPlayerCardsResultValueController>().chekForHighCard(
				GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
				GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			       newCardList);
		
		if(logResult) {
		  Debug.Log("***BBPlayerCardsResultValueController*** chekForHighCard ---> : " + HighCard + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
				Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
				Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
				GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.HighCard.ToString() + " : " + HighCard.ToString();
		}


		maxCardValueRet = HighCard;

		return CardsValues.HighCard;

	}



	public CardsValues getCardsResult(int playerID,out int maxCardValueRet) {

	//CardsValues CValuesRet = CardsValues.HighCard;

	bool tmpRes = false;
    int retSeed = 0;
	maxCardValueRet = 0;
	int maxCardValue = 0;

	//int cardsListLength = 0;
	Vector2[] newCardList = null;

//		Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% phase : " + GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail);


		     if(GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ClosingPreFlop || 
			    GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Flop ||
			    GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDownAllInOnFlop) {

		        newCardList = new Vector2[3];
			    newCardList[0] = GetComponent<BBGameController>().flopCardList[0];
			    newCardList[1] = GetComponent<BBGameController>().flopCardList[1];
			    newCardList[2] = GetComponent<BBGameController>().flopCardList[2];
		     }
	         else if(GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Turn ||
			         GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDownAllInOnTurn) {
			    newCardList = new Vector2[4];
			    newCardList[0] = GetComponent<BBGameController>().flopCardList[0];
			    newCardList[1] = GetComponent<BBGameController>().flopCardList[1];
			    newCardList[2] = GetComponent<BBGameController>().flopCardList[2];
			    newCardList[3] = GetComponent<BBGameController>().turnCard;
	         }
		     else if(GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.River ||
			         GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDown ||
			         GetComponent<BBGameController>()._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.ShowDownAllInOnRiver) {
			    newCardList = new Vector2[5];
			    newCardList[0] = GetComponent<BBGameController>().flopCardList[0];
			    newCardList[1] = GetComponent<BBGameController>().flopCardList[1];
			    newCardList[2] = GetComponent<BBGameController>().flopCardList[2];
			    newCardList[3] = GetComponent<BBGameController>().turnCard;
			    newCardList[4] = GetComponent<BBGameController>().riverCard;
	         }



	if(simulateHighCard) goto TestHighCard;

	tmpRes = chekForRoyalFlush(GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		                       GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			                   newCardList,out retSeed);
 
		if(logResult) {
		  Debug.Log("***BBPlayerCardsResultValueController*** chekForRoyalFlush ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
		  GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.RoyalFlush.ToString();
		}

		if(tmpRes) {
			maxCardValueRet = retSeed;
		    return CardsValues.RoyalFlush;
		}


	tmpRes = chekForStraightFlush(GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		                  GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			              newCardList,out retSeed);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForStraightFlush ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.StraightFlush.ToString() + " : " + retSeed.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = retSeed;
		  return CardsValues.StraightFlush;
		 }

	maxCardValue = 0;
	tmpRes = chekForPoker(
	         GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		     GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForPoker ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Poker.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Poker;
		 }

	maxCardValue = 0;
	tmpRes = chekForFullHouse(
	         GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		     GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForFullHouse ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.FullHouse.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.FullHouse;
		 }

     maxCardValue = 0;
	 tmpRes = chekForFlush(
	          GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		      GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			  newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForFlush ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Flush.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Flush;
		 }

	maxCardValue = 0;
	tmpRes = chekForStraight(
	         GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		     GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForStraight ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Straight.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Straight;
		 }

	maxCardValue = 0;
	tmpRes = chekForThreeOfAkind(
	         GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		     GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForThreeOfAkind ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.ThreeOfAkind.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.ThreeOfAkind;
		 }

	maxCardValue = 0;
	tmpRes = chekForTwoPair(
	         GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		     GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForTwoPair ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.TwoPair.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.TwoPair;
		 }

	maxCardValue = 0;
	tmpRes = chekForPair(
	         GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		     GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			 newCardList,out maxCardValue);

		if(logResult) {
			Debug.Log("***BBPlayerCardsResultValueController*** chekForPair ---> : " + tmpRes + " seed : " + retSeed + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.Pair.ToString() + " : " + maxCardValue.ToString();
		}

		if(tmpRes) {
		  maxCardValueRet = maxCardValue;
		  return CardsValues.Pair;
		 }
    
TestHighCard:
        
	int HighCard = GetComponent<BBPlayerCardsResultValueController>().chekForHighCard(
	               GetComponent<BBGameController>().playerDataList[playerID].card_1_Value,
		           GetComponent<BBGameController>().playerDataList[playerID].card_2_Value,
			       newCardList);
		
		if(logResult) {
		  Debug.Log("***BBPlayerCardsResultValueController*** chekForHighCard ---> : " + HighCard + " playerID : " + playerID);
		  foreach(Vector2 v in newCardList) Debug.Log("*************************************************************************************************** ---> : " + v);
		  Debug.Log("*********************************************card 1****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_1_Value);
		  Debug.Log("*********************************************card 2****************************************************** ---> : " + GetComponent<BBGameController>().playerDataList[playerID].card_2_Value);
			GetComponent<BBGameController>().playerDataList[playerID].T_PlayerMoneyTotal.text = CardsValues.HighCard.ToString() + " : " + HighCard.ToString();
		}


		maxCardValueRet = HighCard;

		return CardsValues.HighCard;

	}


}
}