using UnityEngine;
using UnityEngine.UI;

public class Rule_ItemCollection : MonoBehaviour, IObjective
{
    [SerializeField] Text _itemCountLabel, _timeLimitLabel;
    [SerializeField] float _timeLimit = 20;

    Item[] _items = null;
    int _itemCount = int.MaxValue; // 残りのアイテム数
    float _currentTime;


    public bool IsDone => IsSuccess || TimeOver;

    public bool IsSuccess => _itemCount == 0;

    public bool TimeOver => _currentTime > _timeLimit;

    public void SetEnabled(bool value)
    {
        enabled = value;
    }

    void Start()
    {
        _items = GameObject.FindObjectsOfType<Item>();
    }


    void Update()
    {
        _currentTime += Time.deltaTime;
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
        _itemCountLabel.text = $"ノコリ { _itemCount } コ";
        _timeLimitLabel.text = $"ノコリ { Mathf.RoundToInt( Mathf.Max(0, _timeLimit - _currentTime)) } ビョウ";
    }
}