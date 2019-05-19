using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class Species
{
    public string name;
    public float amount;
    public Vector3 median;
    public List<Creature> creatures = new List<Creature>();

    public Species(string name)
    {
        this.name = name;// RandomWordGenerator.Generate();
    }
}

public class SimulationManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public Creature startCreature;
    public Text information;
    [Space(10)]
    public float speciesDistance = 0.3f;
    public int foodAmount = 100;
    public float foodRange = 40f;
    [Space(10)]
    public List<Creature> creatures = new List<Creature>();
    public List<Species> species = new List<Species>();
    [Space(10)]
    public float mutationRate = 0.3f;
    public float mutationMinRate = 0.05f;
    public float mutationDecreaseCoef = 0.4f;

    private string status_;
    private StateMachine ssm_;

    private void Start()
    {
        UnityEvent.Init();

        ssm_ = new StateMachine("Simulation State Machine", debug: false);

        ssm_.AddState("Phase:Active", StartActivePhase)
            .AddTransition("Phase:Reproduction", IsAllDone, sleep: .5f);

        ssm_.AddState("Phase:Reproduction", StartReproductionPhase)
            .AddTransition("Phase:Active", () => true, sleep: .5f);

        creatures.Add(startCreature);
        
        ssm_.Start("Phase:Active");
    }

    private bool IsAllDone()
    {
        bool done = true;

        foreach(var creature in creatures)
        {
            if(!creature.gameObject.activeSelf)
            {
                //  whatever
                //  Log.Warning("WTF!!!!");
                creature.done = true;
            }
            if (!creature.done)
            {
                done = false;
                break;
            }
        }

        return done;
    }

    private void StartActivePhase()
    {
        UpdateInfo();

        for (int i = 0; i < foodAmount; i++)
        {
            Vector2 circle = Random.insideUnitCircle * foodRange;
            GameObject.Instantiate(foodPrefab).transform.position = new Vector3(circle.x, 0, circle.y);
        }

        foreach (var creature in creatures)
            creature.Activate();
    }
    
    private List<Creature> oldCreatures = new List<Creature>();
    private List<Creature> newCreatures = new List<Creature>();
    private void StartReproductionPhase()
    {
        UpdateInfo();

        oldCreatures.Clear();
        newCreatures.Clear();

        foreach (var creature in creatures)
        {
            switch(creature.food)
            {
                case 0:
                    GameObject.Destroy(creature.gameObject);
                    oldCreatures.Add(creature);
                    break;
                case 1:
                    break;
                case 2:
                    Creature newOne = GameObject.Instantiate<Creature>(creature);
                    newCreatures.Add(newOne);
                    newOne.Mutate(mutationRate);
                    break;
            }
        }

        mutationRate = Mathf.Lerp(mutationRate, mutationMinRate, mutationDecreaseCoef);

        creatures.RemoveRange(oldCreatures);
        creatures.AddRange(newCreatures);
    }

    private void UpdateInfo()
    {
        Clasterization();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ssm_.State);
        sb.AppendFormat("Population: {0}\n", creatures.Count);
        sb.AppendFormat("Speed: min {0} max {1}\n", Creature.minSpeed, Creature.maxSpeed);
        sb.AppendLine();
        sb.AppendLine("Species:");
        foreach (var spec in species)
            sb.AppendFormat("{0}:\n  Median: speed {1:0.0} size {2:0.0} sense {3:0.0}\n  Population: {4}\n",
                spec.name,
                spec.median.x, spec.median.y, spec.median.z,
                spec.amount);

        information.text = sb.ToString();
    }
    
    private void Clasterization()
    {
        foreach(var creature in creatures)
            creature.species = null;

        for(int i=0;i< species.Count;i++)
        {
            Species spice = species[i];
            spice.amount = 0;
            spice.creatures.Clear();
            TryAddAllToSpecies(spice, spice.median);
            if (spice.amount == 0)
            {
                species.RemoveAt(i);
                i--;
            }
            RecalculateMedian(spice);
        }

        for (int i = 0; i < creatures.Count; i++)
        {
            var creature = creatures[i];
            if (creature.species == null)
            {
                Species newSpec = new Species(RandomWordGenerator.GenerateName());
                newSpec.median = creature.parameters.ToVector3();
                species.Add(newSpec);
                TryAddAllToSpecies(newSpec, newSpec.median);
            }
        }
    }

    private void RecalculateMedian(Species spice)
    {
        Vector3 median = Vector3.zero;
        foreach (var creature in spice.creatures)
            median += creature.parameters.ToVector3();
        spice.median = median / spice.creatures.Count;
    }

    private void TryAddAllToSpecies(Species spice, Vector3 position)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            var creature = creatures[i];

            if (creature.species != null) continue;

            if (Vector3.Distance(position, creature.parameters.ToVector3()) < speciesDistance)
            {
                creature.species = spice;
                spice.amount++;
                spice.creatures.Add(creature);
                TryAddAllToSpecies(spice, creature.parameters.ToVector3());
            }
        }
    }
}
