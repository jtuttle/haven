using System.Collections;

public interface IJsonable {
    Hashtable ToJson();
    void FromJson(Hashtable json);
}