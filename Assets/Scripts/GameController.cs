using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField] GameObject _youWinUI, _youLose, _gameStartUI;

    IEnumerator Start()
    {
        // ゲームが使用するオブジェクトを収集
        var objective = GetComponent<IObjective>();
        var car = FindObjectOfType<Car>();

        // ゲームイントロの設定
        objective.SetEnabled(false);
        car?.SetControllable(false);
        _youWinUI?.SetActive(false);
        _youLose?.SetActive(false);
        _gameStartUI?.SetActive(true);
        yield return new WaitForSeconds(3);

        // ゲーム中の設定
        objective.SetEnabled(true);
        car?.SetControllable(true);
        _gameStartUI?.SetActive(false);
        // アイテム数が0になるまで処理を遅延
        yield return new WaitUntil(() => objective.IsDone);

        // ゲーム終了の設定
        objective.SetEnabled(false);
        if( objective.IsSuccess )
            _youWinUI?.SetActive(true);
        else
            _youLose?.SetActive(true);
    }
}
