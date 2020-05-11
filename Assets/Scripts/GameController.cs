using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{

    [SerializeField] GameObject _youWinUI, _youLose, _gameStartUI;
    [SerializeField] SceneSet currentScene, nextScene;

    IEnumerator Start()
    {
        // ゲームが使用するオブジェクトを収集
        var objective = GameObject.FindWithTag("Objective").GetComponent<IObjective>();
        var player = GameObject.FindWithTag("Player").GetComponent<IPlayer>();

        // ゲームイントロの設定
        objective?.SetEnabled(false);
        player?.SetControllable(false);
        _youWinUI?.SetActive(false);
        _youLose?.SetActive(false);
        _gameStartUI?.SetActive(true);
        yield return new WaitForSeconds(3);

        // ゲーム中の設定
        objective?.SetEnabled(true);
        player?.SetControllable(true);
        _gameStartUI?.SetActive(false);
        // アイテム数が0になるまで処理を遅延
        yield return new WaitUntil(() => objective.IsDone);

        // ゲーム終了の設定
        objective?.SetEnabled(false);
        if( objective.IsSuccess )
            _youWinUI?.SetActive(true);
        else
            _youLose?.SetActive(true);

        yield return new WaitForSeconds(5);

        currentScene.Unload();
        nextScene.Load();
    }
}
