using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Item[] _items = null;
    Car _car = null;

    int _itemCount = int.MaxValue;

    [SerializeField] Text _label;

    [SerializeField] GameObject _youWinUI, _gameStartUI;

    IEnumerator Start()
    {
        // ゲームが使用するオブジェクトを収集
        _items = FindObjectsOfType<Item>();
        _car = FindObjectOfType<Car>();

        // ゲームイントロの設定
        _car?.SetControllable(false);
        _youWinUI?.SetActive(false);
        _gameStartUI?.SetActive(true);
        yield return new WaitForSeconds(3);

        // ゲーム中の設定
        _car?.SetControllable(true);
        _gameStartUI?.SetActive(false);
        // アイテム数が0になるまで処理を遅延
        yield return new WaitUntil(() => _itemCount == 0);

        // ゲーム終了の設定
        _youWinUI?.SetActive(true);
    }

    void Update()
    {
        // 情報の更新は5フレームに1回
        if( Time.frameCount % 5 != 0) return;

        // 残りのアイテム数を計算
        var contactItemCount = 0;
        foreach( var item in _items)
        {
            if( item.IsContacted )
                contactItemCount ++;
        }
        _itemCount =  _items.Length - contactItemCount;
        _label.text = $"ノコリ { _itemCount } コ";
    }

}
