using Mono.Data.Sqlite;
using System.Collections;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SqliteManager : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerSaveData playerSaveData;

    private string dbPath;

    private static bool areGameDataWillLoad = false;

    private void Awake()
    {
        dbPath =
            "URI=file:" +
            Application.persistentDataPath +
            "/savegame.db";

        CreateTables();
    }

    private void Start()
    {
        if (areGameDataWillLoad == true)
        {
            LoadData();
        }
    }

    //==================================================
    // Use the Data
    //==================================================
    
    public void SaveData()
    {
        SavePlayer();
        SaveAllColorObjects();
    }

    public void LoadData()
    {
        Debug.Log("LOAD DATA BUTTON CLICKED");

        if (SaveFileExists())
        {
            StartCoroutine(AfterTimeToLoad(1));
           
        }
        else
        {
            Debug.LogWarning("NO SAVE FILE FOUND");
        }
    }

    public static bool SaveFileExists()
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.db");

        return File.Exists(path);
    }

   
    //==================================================
    // CREATE DATABASE TABLE
    //==================================================

    private void CreateTables()
    {
        using (IDbConnection connection = OpenConnection())
        {
            using (IDbCommand command =
                   connection.CreateCommand())
            {
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS PlayerSave
                (
                    id INTEGER PRIMARY KEY,
                    posX REAL,
                    posY REAL,
                    posZ REAL,
                    colorStatus TEXT,
                    health REAL
                );
                
                
                CREATE TABLE IF NOT EXISTS ColorObjectSave
                 (
                    objectId TEXT PRIMARY KEY,
                    posX REAL,
                    posY REAL,
                    posZ REAL,
                    colorStatus TEXT,
                    isTrigger INTEGER
                  );";

                command.ExecuteNonQuery();

                
            }
        }
    }

    //==================================================
    // OPEN DATABASE CONNECTION
    //==================================================

    private IDbConnection OpenConnection()
    {
        IDbConnection connection =
            new SqliteConnection(dbPath);

        connection.Open();

        return connection;
    }

    //==================================================
    // SAVE PLAYER
    //==================================================

    public void SavePlayer()
    {
        if (playerSaveData == null)
        {
            Debug.LogWarning
            (
                "No PlayerSaveData assigned."
            );

            return;
        }

        string colorName = "";

        if (playerSaveData.playerColor != null &&
            playerSaveData.playerColor.PlayerCurrentColor != null)
        {
            colorName =
            playerSaveData
            .playerColor
            .PlayerCurrentColor
            .currentColorName;
        }

        using (IDbConnection connection =
               OpenConnection())
        {
            using (IDbCommand command =
                   connection.CreateCommand())
            {
                command.CommandText =
                @"
                INSERT OR REPLACE INTO PlayerSave
                (
                    id,
                    posX,
                    posY,
                    posZ,
                    colorStatus,
                    health
                )

                VALUES
                (
                    1,
                    @x,
                    @y,
                    @z,
                    @color,
                    @health
                );
                ";

                AddParam
                (
                    command,
                    "@x",
                    playerSaveData.transform.position.x
                );

                AddParam
                (
                    command,
                    "@y",
                    playerSaveData.transform.position.y
                );

                AddParam
                (
                    command,
                    "@z",
                    playerSaveData.transform.position.z
                );

                AddParam
                (
                    command,
                    "@color",
                    colorName
                );

                AddParam
                (
                    command,
                    "@health",
                    playerSaveData.playerHealth.PlayerCurrentHealth
                );

                command.ExecuteNonQuery();
            }
        }

        Debug.Log("PLAYER SAVED");
    }

    //==================================================
    // LOAD PLAYER
    //==================================================

    public void LoadPlayer()
    {
        using (IDbConnection connection = OpenConnection())
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText =
                @"
            SELECT posX, posY, posZ, colorStatus, health
            FROM PlayerSave
            WHERE id = 1;
            ";

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Vector3 loadedPosition = new Vector3
                        (
                            System.Convert.ToSingle(reader["posX"]),
                            System.Convert.ToSingle(reader["posY"]),
                            System.Convert.ToSingle(reader["posZ"])
                        );

                        string loadedColor =
                            reader["colorStatus"].ToString();

                        float loadedHealth =
                            System.Convert.ToSingle(reader["health"]);

                        playerSaveData.ApplyLoadedData
                        (
                            loadedPosition,
                            loadedColor,
                            loadedHealth
                        );

                        Debug.Log("PLAYER LOADED");
                    }
                    else
                    {
                        Debug.LogWarning("NO SAVE DATA FOUND");
                    }
                }
            }
        }
    }


    //==================================================
    // Save ColorObject
    //==================================================

    private void SaveAllColorObjects()
    {
        ColorObjectSaveData[] objects =
        FindObjectsOfType<ColorObjectSaveData>();

        foreach (ColorObjectSaveData obj in objects)
        {
            // If some object did not provide id or didnt assign skip and continous to next
            if (string.IsNullOrEmpty(obj.objectId))
            {
                Debug.LogWarning(obj.name + " has no objectId. Skipped.");
                continue;
            }

            //Connect to the database
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                       INSERT OR REPLACE INTO ColorObjectSave
                        (
                          objectId,
                          posX,
                          posY,
                          posZ,
                          colorStatus,
                          isTrigger
                        )
                        VALUES
                        (
                          @id,
                          @x,
                          @y,
                          @z,
                          @color,
                          @trigger
                         
                         ); ";

                    AddParam(command, "@id", obj.objectId);
                    AddParam(command, "@x", obj.transform.position.x);
                    AddParam(command, "@y", obj.transform.position.y);
                    AddParam(command, "@z", obj.transform.position.z);
                    AddParam(command, "@color", obj.GetColorName());
                    AddParam(command, "@trigger", obj.GetTriggerState() ? 1 : 0);

                    command.ExecuteNonQuery();
                }
            }


        }
        Debug.Log("COLOR OBJECTS SAVED");
    }

    //==================================================
    // LOAD ColorOBject
    //==================================================
    private void LoadAllColorObjects()
    {
        ColorObjectSaveData[] objects =
            FindObjectsOfType<ColorObjectSaveData>();

        foreach (ColorObjectSaveData obj in objects)
        {
            if (string.IsNullOrEmpty(obj.objectId))
            {
                continue;
            }

            using (IDbConnection connection = OpenConnection())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                SELECT posX, posY, posZ, colorStatus, isTrigger
                FROM ColorObjectSave
                WHERE objectId = @id;
                ";

                    AddParam(command, "@id", obj.objectId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Vector3 loadedPosition = new Vector3
                            (
                                System.Convert.ToSingle(reader["posX"]),
                                System.Convert.ToSingle(reader["posY"]),
                                System.Convert.ToSingle(reader["posZ"])
                            );

                            string loadedColor =
                                reader["colorStatus"].ToString();

                            bool loadedIsTrigger =
                                System.Convert.ToInt32(reader["isTrigger"]) == 1;

                            obj.ApplyLoadedData
                            (
                                loadedPosition,
                                loadedColor,
                                loadedIsTrigger
                            );
                        }
                    }
                }
            }
        }

        Debug.Log("COLOR OBJECTS LOADED");
    }

    //==================================================
    // ADD SQL PARAMETER
    //==================================================

    private void AddParam
    (
        IDbCommand command,
        string name,
        object value
    )
    {
        IDbDataParameter param =
            command.CreateParameter();

        param.ParameterName = name;

        param.Value = value;

        command.Parameters.Add(param);
    }


    //==================================================
    // Helper Method
    //==================================================

    private IEnumerator AfterTimeToLoad(float value)
    {
        yield return new WaitForSeconds(value);
        LoadPlayer();
        LoadAllColorObjects();
        areGameDataWillLoad = false;
        Debug.Log("DELAY LOAD COMPLETE");
    }

    public static void SetGameDataWillLoad(bool value)
    {
        areGameDataWillLoad = value;
    }
}
