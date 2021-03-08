using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Transition
//{
//    SawPlayer = 0, //看到玩家
//    ReachPlayer,//接近玩家，即玩家在攻击范围内
//    LostPlayer,//玩家离开视线
//    NoHealth,//死亡
//}

///// <summary>
///// Place the labels for the States in this enum.
///// Don't change the first label, NullTransition as FSMSystem class uses it.
///// </summary>
//public enum StateID
//{
//    Patrolling=0,//巡逻的状态编号为0
//    Chasing,//追逐的状态编号为1
//    Attacking,//攻击状态 2
//    Dead,//死亡状态 3
//}


//public abstract class FSMState
//{
//    //每一项都记录了一个“转换-状态”对的信息
//    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
//    //状态编号ID
//    protected StateID stateID;
//    public StateID ID { get { return stateID; } }

//    public void AddTransition(Transition trans, StateID id)
//    {
//        // Check if anyone of the args is invalid
//        if (trans == Transition.NullTransition)
//        {
//            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
//            return;
//        }

//        if (id == StateID.NullStateID)
//        {
//            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
//            return;
//        }

//        // Since this is a Deterministic FSM,
//        //   check if the current transition was already inside the map
//        if (map.ContainsKey(trans))
//        {
//            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
//                           "Impossible to assign to another state");
//            return;
//        }

//        map.Add(trans, id);
//    }

//    /// <summary>
//    /// This method deletes a pair transition-state from this state's map.
//    /// If the transition was not inside the state's map, an ERROR message is printed.
//    /// </summary>
//    public void DeleteTransition(Transition trans)
//    {
//        // Check for NullTransition
//        if (trans == Transition.NullTransition)
//        {
//            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
//            return;
//        }

//        // Check if the pair is inside the map before deleting
//        if (map.ContainsKey(trans))
//        {
//            map.Remove(trans);
//            return;
//        }
//        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
//                       " was not on the state's transition list");
//    }

//    /// <summary>
//    /// This method returns the new state the FSM should be if
//    ///    this state receives a transition and 
//    /// </summary>
//    public StateID GetOutputState(Transition trans)
//    {
//        // Check if the map has this transition
//        if (map.ContainsKey(trans))
//        {
//            return map[trans];
//        }
//        return StateID.NullStateID;
//    }

//    /// <summary>
//    /// This method is used to set up the State condition before entering it.
//    /// It is called automatically by the FSMSystem class before assigning it
//    /// to the current state.
//    /// </summary>
//    public virtual void DoBeforeEntering() { }

//    /// <summary>
//    /// This method is used to make anything necessary, as reseting variables
//    /// before the FSMSystem changes to another one. It is called automatically
//    /// by the FSMSystem before changing to a new state.
//    /// </summary>
//    public virtual void DoBeforeLeaving() { }

//    /// <summary>
//    /// This method decides if the state should transition to another on its list
//    /// NPC is a reference to the object that is controlled by this class
//    /// </summary>
//    public abstract void Reason(GameObject player, GameObject npc);

//    /// <summary>
//    /// This method controls the behavior of the NPC in the game World.
//    /// Every action, movement or communication the NPC does should be placed here
//    /// NPC is a reference to the object that is controlled by this class
//    /// </summary>
//    public abstract void Act(GameObject player, GameObject npc);

//}
