using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    // Hold list of Actors (Player and Bots) for managing.
    public class ActorsManager : MonoBehaviour
    {
        public List<Actor> Actors { get; private set; }
        public GameObject Player { get; private set; }

        public void SetPlayer(GameObject player) => Player = player;

        void Awake()
        {
            Actors = new List<Actor>();
        }
    }
}
