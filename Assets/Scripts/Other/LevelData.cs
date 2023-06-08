[System.Serializable]
public class LevelData
{
    public string levelName;
    public Field[] field;
    public LogicField[] logicField;

    public LevelData(string levelName, Field[] field, LogicField[] logicFields)
    {
        this.levelName = levelName;
        this.field = field;
        this.logicField = logicFields;
    }
}

[System.Serializable]
public class Field
{
    public int xCoordinate;
    public int yCoordinate;

    public string type;
    public string specification;

    public Field(int xCoordinate, int yCoordinate, string type, string specification)
    {
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.type = type;
        this.specification = specification;
    }
}

[System.Serializable]
public class  LogicField
{
    public SensorInput[] sensorInputs;
    public LogicGate[] logicGates;
    public SensorOutput[] sensorOutputs;

    public LogicField(SensorInput[] sensorInputs, LogicGate[] logicGates, SensorOutput[] sensorOutputs)
    {
        this.sensorInputs = sensorInputs;
        this.logicGates = logicGates;
        this.sensorOutputs = sensorOutputs;
    }
}

[System.Serializable]
public class SensorInput
{
    public string type;
    public string specificationOne;
    public string specificationTwo;
    public string specificationThree;

    public SensorInput(string type, string specificationOne, string specificationTwo, string specificationThree)
    {
        this.type = type;
        this.specificationOne = specificationOne;
        this.specificationTwo = specificationTwo;
        this.specificationThree = specificationThree;
    }
}

[System.Serializable]
public class LogicGate
{
    public int row;
    public int position;
    public string type;
    public int[] inputs;

    public LogicGate(int row, int position, string type, int[] inputs)
    {
        this.row = row;
        this.position = position;
        this.type = type;
        this.inputs = inputs;
    }
}

[System.Serializable]
public class SensorOutput
{
    public string type;
    public string specification;
    public int[] inputs;

    public SensorOutput(string type, string specification, int[] inputs)
    {
        this.type = type;
        this.specification = specification;
        this.inputs = inputs;
    }
}
