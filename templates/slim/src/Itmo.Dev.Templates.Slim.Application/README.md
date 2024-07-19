# Application

Application layer is structured as Vertical Slice Architecture

You should use top level folders to define an application features, 
folders nested in them should denote an interaction kind of feature

Sample:

```
Application
    - Users
        - Endpoints
        - Kafka
        - BackgroundTasks
```

See README files in each folder of interaction kinds in a sample feature
provided by template for more info.