using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryManager : MonoBehaviour
{
    Scene simulationScene;
    PhysicsScene2D physicsScene;

    public Transform obstaclesTransform;
    public LayerMask ignoreRaycast;

    Dictionary<GameObject, GameObject> dynamicObjects;

    void Start()
    {
        dynamicObjects = new Dictionary<GameObject, GameObject>();
        CreatePhysicsScene();
    }

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("TrajectorySimulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach (Transform obj in obstaclesTransform)
        {
            var go = Instantiate(obj.gameObject, obj.position, obj.rotation);

            foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
            
            SceneManager.MoveGameObjectToScene(go, simulationScene);

            var combined = obj.GetComponentsInChildren<Transform>().Zip(go.GetComponentsInChildren<Transform>(), (real, fake) => (real, fake));

            foreach (var child in combined)
            {
                if (!child.real.gameObject.isStatic)
                {
                    dynamicObjects.Add(child.real.gameObject, child.fake.gameObject);
                }
            }
        }
    }

    public LineRenderer lineBackup;
    public int maxIterations = 100;

    public void SimulateTrajectory(Projectile projectile, Vector3 position, Vector3 velocity, int maxBounces)
    {
        var go = Instantiate(projectile, position, Quaternion.identity);
        try { go.GetComponent<Renderer>().enabled = false; } catch { }
        go.GetComponentInChildren<Collider2D>().isTrigger = projectile.GetComponent<Placeable>().isTriggerByDefault;
        go.isSimulatingTrajectory = true;
        SceneManager.MoveGameObjectToScene(go.gameObject, simulationScene);

        go.Init(velocity);

        LineRenderer line = projectile.GetComponent<LineRenderer>();
        if (!line)
        {
            line = lineBackup;
        }

        line.positionCount = maxIterations+1;
        line.SetPosition(0, go.transform.position);

        int lastBounces = 0;
        for (int i = 0; i < maxIterations; i++)
        {
            if (lastBounces != go.bouncesCount)
            {
                lastBounces = go.bouncesCount;

                int index = Mathf.Max(i, 2);
                line.positionCount = index+1;
                
                Vector3 start = line.GetPosition(index-1);
                Vector2 dir = start - line.GetPosition(index-2);
                RaycastHit2D hit = Physics2D.Raycast(start, dir, Mathf.Infinity, ~ignoreRaycast);

                if (hit.collider != null)
                {
                    line.SetPosition(index, hit.point);
                }
                else
                {
                    line.SetPosition(index, start);
                }

                break;
                // if (lastBounces >= maxBounces)
                // {
                //     // int index = Mathf.Max(i-1, 1);
                //     // line.positionCount = index+1;
                //     // line.SetPosition(index, go.transform.position);
                //     break;
                // }
            }

            physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i+1, go.transform.position);
        }

        Destroy(go.gameObject);
    }

    public void SimulateShield(Shield shield)
    {
        GameObject go;
        if (dynamicObjects.TryGetValue(shield.gameObject, out go))
        {
            go.transform.position = shield.transform.position;
            go.transform.rotation = shield.transform.rotation;
            return;
        }
        
        go = Instantiate(shield, shield.transform.position, shield.transform.rotation).gameObject;
        go.GetComponentInChildren<Collider2D>().isTrigger = false;
        dynamicObjects.Add(shield.gameObject, go);
        SceneManager.MoveGameObjectToScene(go, simulationScene);
    }

    public void UpdateObject(GameObject obj)
    {
        GameObject go;
        if (dynamicObjects.TryGetValue(obj, out go))
        {
            go.transform.position = obj.transform.position;
            go.transform.rotation = obj.transform.rotation;
        }
    }

    public void RemoveObject(GameObject obj)
    {
        GameObject go;
        if (dynamicObjects.TryGetValue(obj, out go))
        {
            dynamicObjects.Remove(obj);
            Destroy(go);
        }
    }
}

