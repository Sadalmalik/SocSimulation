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
        public Color fishColor = new Color(.35f, .85f, .65f);
        public Color fishermanColor = new Color(.85f, .65f, .35f);
        [Header("Параметры симуляции")]
        [Space(10)]
        public int StartFishCount = 100;
        public float FishLifeTime = 30;
        public float ReproductionDelay = 10;
        public int ReproduceAmount = 2;

        [Space(10)]
        public int StartFishermanCount = 5;
        public float FishermanLifetime = 100;
        public int FishermanNeed = 3;
        public int FishermanHungerTime = 10;
        public bool Stockpile = false;
        
        [Space(10)]
        public float radius = 45;

        private Pool<Fish> fishPool;
        private Pool<FisherMan> fishermanPool;

        private List<Fish> fishs;
        private List<FisherMan> fishermans;

        void Start()
        {
            fishs = new List<Fish>();
            fishermans = new List<FisherMan>();

            fishPool = new Pool<Fish>(
                    () => new Fish(CreateMob(0.4f, 1, 2, fishColor)));

            fishermanPool = new Pool<FisherMan>(
                () => new FisherMan(CreateMob(1.2f, 2, 1, fishermanColor)));

            Fish.CreateFish = CreateFish;

            ApplySettings();

            for (int i = 0; i < StartFishCount; i++)
                fishs.Add(fishPool.Get());

            for (int i=0;i< StartFishermanCount;i++)
                fishermans.Add(fishermanPool.Get());
        }

        private void CreateFish(Vector3 position)
        {
            var fish = fishPool.Get();
            fish.Mob.transform.position = position;
            fish.Born();
            fishs.Add(fish);
        }

        private Mob CreateMob(float size = 1, float sense = 1, float speed = 1, Color color = default)
        {
            Mob mob = GameObject.Instantiate<Mob>(prefab);
            mob.ApplyParams(size, sense);
            mob.SetColor(color);
            MoveRandomInCircle behaviour = mob.gameObject.AddComponent<MoveRandomInCircle>();
            behaviour.mob = mob;
            behaviour.speed = speed;
            behaviour.range = radius;
            Vector3 position = UnityEngine.Random.insideUnitCircle * radius;
            behaviour.transform.position = new Vector3(position.x, 0, position.y);
            return mob;
        }

        //====================================================================================================//

        private void FixedUpdate()
        {
            ApplySettings();

            foreach (var mob in fishs)
                mob.FixedUpdate();

            foreach (var mob in fishermans)
                mob.FixedUpdate();
            
            for (int i = fishs.Count - 1; i >= 0; i--)
                if (fishs[i].dead)
                {
                    fishPool.Free(fishs[i]);
                    fishs.RemoveAt(i);
                }

            for (int i = fishermans.Count - 1; i >= 0; i--)
                if (fishermans[i].dead)
                {
                    fishermanPool.Free(fishermans[i]);
                    fishermans.RemoveAt(i);
                }

            UpdateInformation();
        }

        private void ApplySettings()
        {
            Fish.lifeTime = FishLifeTime;
            Fish.reproductionDelay = ReproductionDelay;
            Fish.reproduceAmount = ReproduceAmount;

            FisherMan.lifeTime = FishermanLifetime;
            FisherMan.hungerDelay = FishermanHungerTime;
            FisherMan.collectLimit = FishermanNeed;
            FisherMan.stockpile = Stockpile;
        }

        private void UpdateInformation()
        {
            text.text = string.Format("Simulation statistics:\n\nFish amount: {0}\nFishermans: {1}", fishs.Count, fishermans.Count);
        }
    }
}