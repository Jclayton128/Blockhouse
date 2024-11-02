using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ProjectileLibrary : SerializedMonoBehaviour
{
    public static ProjectileLibrary Instance { get; private set; }  

    [SerializeField] Dictionary<ProjectileHandler.ProjectileType, ProjectileHandler> _projectileMenu = null;

    private void Awake()
    {
        Instance = this;
    }


    public ProjectileHandler GetProjectileFromType(ProjectileHandler.ProjectileType pType)
    {
        if (_projectileMenu.ContainsKey(pType))
        {
            return _projectileMenu[pType];
        }
        else
        {
            Debug.LogWarning("Menu doesn't contain " + pType);
            return null;
        }
    }
}
