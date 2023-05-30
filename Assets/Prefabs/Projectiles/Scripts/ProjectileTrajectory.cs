using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileTrajectory : MonoBehaviour
{

    Scene simulationScene;
    PhysicsScene2D physicsScene;

    public Transform obstaclesTransform;

    void Start()
    {
        CreatePhysicsScene();
    }

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach (Transform obj in obstaclesTransform)
        {
            var go = Instantiate(obj.gameObject, obj.position, obj.rotation);
            go.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(go, simulationScene);
        }
    }

    public LineRenderer line;
    public int maxIterations = 100;

    public void SimulateTrajectory(Projectile projectile, Vector3 position, Vector3 velocity)
    {
        var go = Instantiate(projectile, position, Quaternion.identity);
        go.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(go.gameObject, simulationScene);

        go.Init(velocity);

        line.positionCount = maxIterations;
        for (int i = 0; i < maxIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i, go.transform.position);
        }

        Destroy(go.gameObject);
    }
}

