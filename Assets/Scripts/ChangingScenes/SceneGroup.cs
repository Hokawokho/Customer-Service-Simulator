using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine;

namespace Systems.SceneManagement{

    [Serializable]
    public class SceneGroup{

        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        public string FindSceneNameByType(SceneType sceneType){

            return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Reference.Name;
        }
    }

    [Serializable]
    public class SceneData{
        public SceneReference Reference;
        public String Name => Reference.Name;
        public SceneType SceneType;

    }

    public enum SceneType {ActiveScene, Game1, Game2, Game3, StartMenu}

}