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

            var combined = obj.GetComponentsInChildren<Transform>().Zip(go.GetComponentsInChildren<Transform>(), (real, fake) => (real, fake));

            foreach (var child in combined)
            {
                if (!child.real.gameObject.isStatic)
                {
                    dynamicObjects.Add(child.real.gameObject, child.fake.gameObject);
                }

                var enemy = child.fake.GetComponent<EnemyBasic>();
                if (enemy)
                {
                    enemy.enabled = false;
                }
            }

            SceneManager.MoveGameObjectToScene(go, simulationScene);
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
        bool recalculateBouncePoint = false;
        for (int i = 0; i < maxIterations; i++)
        {
            if (recalculateBouncePoint)
            {
                recalculateBouncePoint = false;

                if (i >= 3)
                {
                    var v1 = line.GetPosition(i-1) - line.GetPosition(i);
                    var v2 = line.GetPosition(i-2) - line.GetPosition(i-3);

                    float dot = Vector3.Dot(v1.normalized, v2.normalized);
                    if (Mathf.Abs(dot) < 0.99f)
                    { 
                        // Calculate intersection point between lines before and after bounce

                        float a1 = v1.y / v1.x;
                        float c1 = line.GetPosition(i-1).y - a1 * line.GetPosition(i-1).x;
                        
                        float a2 = v2.y / v2.x;
                        float c2 = line.GetPosition(i-2).y - a2 * line.GetPosition(i-2).x;

                        float x = (c2 - c1) / (a1 - a2);
                        // float y = (c2*a1 - c1*a2) / (a1 - a2);
                        float y = a1 * x + c1;
                        
                        var pos = new Vector3(x, y, 0.0f);
                        line.SetPosition(i-1, pos);
                    }
                }
            }

            if (go.bouncesCount > go.simulatedBouncesCount) 
            {
                int index = Mathf.Max(i, 2);
                line.positionCount = index+1;

                // Snap last point to hit collider
                
                Vector3 start = line.GetPosition(index-1);
                Vector3 dir = start - line.GetPosition(index-2);
                RaycastHit2D hit = Physics2D.Raycast(start, dir, 1.0f, ~ignoreRaycast);

                if (hit.collider != null)
                {
                    line.SetPosition(index, hit.point);
                }
                else
                {
                    line.SetPosition(index, start + 0.5f*dir);
                }

                break;
            }
            else if (go.bouncesCount != lastBounces)
            {
                lastBounces = go.bouncesCount;
                recalculateBouncePoint = true;
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

