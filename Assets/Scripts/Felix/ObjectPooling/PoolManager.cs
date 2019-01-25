using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    public void CreatePool(GameObject prefab, int poolSize)
    {
        //each different game objects has different ID. we assign the ID into the poolKey
        int poolKey = prefab.GetInstanceID();

        //check if the dictionary is already containing the poolkey, if not, add it into the dictionary
        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            //OPTIONAL: To let the unity hierarchy panel looks CLEAN!
            //set all the poolObjects as the child of poolManager (so all the objects are hided at hierarchy)
            GameObject poolHolder = new GameObject(prefab.name + " pool"); //e.g. bullet pool
            //bullet pool will be set as a child of PoolManager
            poolHolder.transform.parent = transform;

            //based on the parameter poolsize, we instantiate the game object using the parameter prefab
            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                //add the instantiated object into the dictionary
                poolDictionary[poolKey].Enqueue(newObject);
                //the poolObjects will be set as child of bullet pool (example)                
                newObject.SetParent(poolHolder.transform);
            }
        }
    }

    //
    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        //if the dictionary is containing the specific poolkey, proceed to reuse the gameobject
        if (poolDictionary.ContainsKey(poolKey))
        {
            //assign the dictionary[poolKey] object to the objectToReuse (later we will use the objectToReuse to spawn the gameobject)
            //NOTED: dictionary[poolKey] space is emptied now due to dequeue!
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            //enqueue back the gameobject to the dictionary[poolKey]
            poolDictionary[poolKey].Enqueue(objectToReuse);
            //"spawn" the game object!
            objectToReuse.Reuse(position, rotation);
        }
    }

    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent; //to indicate whether the poolObject type has their poolObjectScript
        PoolObject poolObjectScript; //reference to the poolObject'script. Only has value if "hasPoolObjectComponent" is true

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;

            if (hasPoolObjectComponent)
            {
                poolObjectScript.OnObjectReuse();
            }
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}