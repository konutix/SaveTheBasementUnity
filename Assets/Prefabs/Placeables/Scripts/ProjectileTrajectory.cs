using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileTrajectory : MonoBehaviour
{
    Scene simulationScene;
    PhysicsScene2D physicsScene;

    public Transform obstaclesTransform;

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
                    print("Adding " + child.real.gameObject.name);
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
        go.GetComponent<Renderer>().enabled = false;
        go.GetComponent<Collider2D>().isTrigger = false;
        go.isSimulatingTrajectory = true;
        SceneManager.MoveGameObjectToScene(go.gameObject, simulationScene);

        go.Init(velocity);

        LineRenderer line = projectile.GetComponent<LineRenderer>();
        if (!line)
        {
            line = lineBackup;
        }

        line.positionCount = 4*maxIterations+1;
        line.SetPosition(0, go.transform.position);
        for (int i = 0; i < 4*maxIterations; i++)
        {
            if (go.bouncesCount >= maxBounces)
            {
                line.positionCount = Mathf.Max(i-1, 2);
                break;
            }

            physicsScene.Simulate(0.25f*Time.fixedDeltaTime);
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
        go.GetComponent<Collider2D>().isTrigger = false;
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
}

