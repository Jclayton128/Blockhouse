using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileController : MonoBehaviour
{
    public static ProjectileController Instance { get; private set; }

    //state
    Dictionary<ProjectileHandler.ProjectileType, Queue<ProjectileHandler>> _pooledProjectiles = 
        new Dictionary<ProjectileHandler.ProjectileType, Queue<ProjectileHandler>>();
    Dictionary<ProjectileHandler.ProjectileType, List<ProjectileHandler>> _activeProjectiles = 
        new Dictionary<ProjectileHandler.ProjectileType, List<ProjectileHandler>>();

    private void Awake()
    {
        Instance = this;
    }


    public ProjectileHandler RequisitionActivatedProjectile(ProjectileHandler.ProjectileType pType)
    {
        ProjectileHandler ph;
        if (_pooledProjectiles.ContainsKey(pType) && _pooledProjectiles[pType].Count > 0)
        {
            ph = _pooledProjectiles[pType].Dequeue();
        }
        else
        {
            ph = Instantiate(ProjectileLibrary.Instance.GetProjectileFromType(pType));
            if (!_activeProjectiles.ContainsKey(pType))
            {
                _activeProjectiles.Add(pType, new List<ProjectileHandler>());
            }
            _activeProjectiles[pType].Add(ph);
        }
        ph.Activate();
        return ph;
    }

    public void ReturnDeactivatedProjectile(ProjectileHandler deactivatedProjectileHandler)
    {
        if (!_pooledProjectiles.ContainsKey(deactivatedProjectileHandler.PType))
        {
            _pooledProjectiles.Add(deactivatedProjectileHandler.PType, new Queue<ProjectileHandler>());
        }

        _pooledProjectiles[deactivatedProjectileHandler.PType].Enqueue(deactivatedProjectileHandler);
        //unusedProjectileHandler.Deactivate();
    }
}
