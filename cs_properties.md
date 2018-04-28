# 属性
```cs
class A
{
    private string _a = "";
    public string a //get, set
    {
        get 
        {
            return _a;
        } //可简化成 get => _a;
        set 
        {
            _a = value + "b";
        }
    }
    public string b { get; set; } //auto getter, auto setter
    public string c { get; } //readonly from extern
}
```