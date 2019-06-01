using SimulationBase;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation2
{
    public class SimulationManager : MonoBehaviour
    {
        public Text text;
        public Mob prefab;

        [Header("Параметры рыб")]
        [Header("Параметры симуляции")]
        [Space(10)]
        public MobSettings fishMobSettings;
        public MoveRandomInCircleSettings fishMotionSettings;
        public FishSettings fishSettings;
        public int startFishCount = 20;

        [Header("Параметры рыбаков")]
        [Space(10)]
        public MobSettings fishermanMobSettings;
        public MoveRandomInCircleSettings fishermanMotionSettings;
        public FisherManSettings fisherManSettings;
        public int startFishermanCount = 5;

        [Header("Прочее")]
        [Space(10)]
        public float spawnRadius = 40;

        private MobsManager mobsManager_;

        private Delay infoDelay_ = new Delay();

        void Start()
        {
            infoDelay_.Start();

            mobsManager_ = MobsManager.instance;
            mobsManager_.prefab = prefab;

            for (int i = 0; i < startFishCount; i++)
            {
                Fish newFish = MobsManager.instance.CreateMob<Fish>();
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                newFish.mob.transform.position = new Vector3(offset.x, 0, offset.y);
                newFish.Init(fishMobSettings);
                newFish.Init(fishMotionSettings);
                newFish.Init(fishSettings);
            }

            for (int i=0;i< startFishermanCount; i++)
            {
                FisherMan newFisherman = MobsManager.instance.CreateMob<FisherMan>();
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                newFisherman.mob.transform.position = new Vector3(offset.x, 0, offset.y);
                newFisherman.Init(fishermanMobSettings);
                newFisherman.Init(fishermanMotionSettings);
                newFisherman.Init(fisherManSettings);
            }
        }
        
        //====================================================================================================//

        private void FixedUpdate()
        {
            mobsManager_.Tick();

            if(infoDelay_.IsComplete())
            {
                infoDelay_.Start();
                UpdateInformation();
            }
        }
        
        private void UpdateInformation()
        {
            int fishCount = 0;
            int fishermanCount = 0;

            foreach (var mob in mobsManager_.mobs)
            {
                if (null != mob as Fish)
                    fishCount++;
                if (null != mob as FisherMan)
                    fishermanCount++;
            }

            text.text = string.Format("Simulation statistics:\n\nFish amount: {0}\nFishermans: {1}", fishCount, fishermanCount);
        }
    }
}