using System.Collections;
using System.Collections.Generic;
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
        CreatePhysicsScene();
        dynamicObjects = new Dictionary<GameObject, GameObject>();
    }

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("TrajectorySimulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach (Transform obj in obstaclesTransform)
        {
            var go = Instantiate(obj.gameObject, obj.position, obj.rotation);
            go.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(go, simulationScene);
        }
    }

    public LineRenderer lineBackup;
    public int maxIterations = 100;

    public void SimulateTrajectory(Projectile projectile, Vector3 position, Vector3 velocity, int maxBounces)
    {
        var go = Instantiate(projectile, position, Quaternion.identity);
        go.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(go.gameObject, simulationScene);

        go.Init(velocity);

        LineRenderer line = projectile.GetComponent<LineRenderer>();
        if (!line)
        {
            line = lineBackup;
        }

        line.positionCount = maxIterations+1;
        line.SetPosition(0, go.transform.position);
        for (int i = 0; i < maxIterations; i++)
        {
            if (go.bouncesCount >= maxBounces)
            {
                line.positionCount = Mathf.Max(i-1, 2);
                break;
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
        dynamicObjects.Add(shield.gameObject, go);
        SceneManager.MoveGameObjectToScene(go, simulationScene);
    }
}

