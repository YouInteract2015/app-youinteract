    --
    -- Drop all tables
    --
    
    DROP TABLE "generic_scheduler_items";
    DROP TABLE "generic_config_items";
    DROP TABLE "generic_devices";
    DROP TABLE "generic_scheduler_config_items";
    DROP TABLE "generic_scheduler_configs";
    DROP TABLE "generic_configs";
    DROP TABLE "generic_items";
    DROP TABLE "generic_schedulers";
    DROP TABLE "generic_templates";
    DROP TABLE "kinect_items_download";
    DROP TABLE "kinect_scheduler_items";
    DROP TABLE "kinect_scheduler_configs";
    DROP TABLE "kinect_scheduler_config_items";
    DROP TABLE "kinect_devices";
    DROP TABLE "kinect_config_items";
    DROP TABLE "kinect_configs";
    DROP TABLE "kinect_items";
    DROP TABLE "kinect_schedulers";
    DROP TABLE "kinect_templates";
    DROP TABLE "password_resets";
    DROP TABLE "users";
    
    -- --------------------------------------------------------
    
    --
    -- Estrutura da tabela `users`
    --
     
    CREATE TABLE "users" (
      "id" INT NOT NULL IDENTITY(1,1),
      "name" VARCHAR(255) NOT NULL,
      "email" VARCHAR(255) NOT NULL UNIQUE,
      "password" VARCHAR(60) NOT NULL,
      "remember_token" VARCHAR(100) DEFAULT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `password_resets`
    --
     
    CREATE TABLE "password_resets" (
      "email" VARCHAR(255) NOT NULL,
      "token" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_templates`
    --
     
    CREATE TABLE "kinect_templates" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "path" VARCHAR(255) NOT NULL,
      "preview" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_schedulers`
    --
     
    CREATE TABLE "kinect_schedulers" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_items`
    --
     
    CREATE TABLE "kinect_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "dll" VARCHAR(255) NOT NULL,
      "config" VARCHAR(255) NOT NULL,
      "version" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_configs`
    --
     
    CREATE TABLE "kinect_configs" (
      "id" INT NOT NULL IDENTITY(1,1),
      "template_id" INT NOT NULL,
      "scheduler_id" INT NOT NULL,
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "default" tinyint NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([template_id]) REFERENCES [kinect_templates] ([id]),
      FOREIGN KEY ([scheduler_id]) REFERENCES [kinect_schedulers] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_config_items`
    --
     
    CREATE TABLE "kinect_config_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "config_id" INT NOT NULL,
      "item_id" INT NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "priority" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([config_id]) REFERENCES [kinect_configs] ([id]),
      FOREIGN KEY ([item_id]) REFERENCES [kinect_items] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_devices`
    --
     
    CREATE TABLE "kinect_devices" (
      "id" INT NOT NULL IDENTITY(1,1),
      "config_id" INT NOT NULL,
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "location" VARCHAR(255) NOT NULL,
      "ip" VARCHAR(255) NOT NULL,
      "status" tinyint NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([config_id]) REFERENCES [kinect_configs] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_scheduler_config_items`
    --
     
    CREATE TABLE "kinect_scheduler_config_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "file" VARCHAR(255) NOT NULL,
      "preview" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_scheduler_configs`
    --
     
    CREATE TABLE "kinect_scheduler_configs" (
      "id" INT NOT NULL IDENTITY(1,1),
      "item_id" INT NOT NULL,
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "startat" DATETIME DEFAULT (getdate()) NOT NULL,
      "endat" DATETIME DEFAULT (getdate()) NOT NULL,
      "monday" tinyint NOT NULL DEFAULT '1',
      "tuesday" tinyint NOT NULL DEFAULT '1',
      "wednesday" tinyint NOT NULL DEFAULT '1',
      "thursday" tinyint NOT NULL DEFAULT '1',
      "friday" tinyint NOT NULL DEFAULT '1',
      "saturday" tinyint NOT NULL DEFAULT '1',
      "sunday" tinyint NOT NULL DEFAULT '1',
      "starthour" INT NOT NULL,
      "endhour" INT NOT NULL,
      "startminute" INT NOT NULL,
      "endminute" INT NOT NULL,
      "startsecond" INT NOT NULL,
      "endsecond" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([item_id]) REFERENCES [kinect_scheduler_config_items] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_scheduler_items`
    --
     
    CREATE TABLE "kinect_scheduler_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "config_id" INT NOT NULL,
      "scheduler_id" INT NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "priority" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([config_id]) REFERENCES [kinect_scheduler_configs] ([id]),
      FOREIGN KEY ([scheduler_id]) REFERENCES [kinect_schedulers] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_items_download`
    --
     
    CREATE TABLE "kinect_items_download" (
      "id" INT NOT NULL IDENTITY(1,1),
      "device_id" INT NOT NULL,
      "item_id" INT NOT NULL,
      "version" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([device_id]) REFERENCES [kinect_devices] ([id]),
      FOREIGN KEY ([item_id]) REFERENCES [kinect_items] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_templates`
    --
     
    CREATE TABLE "generic_templates" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "view" VARCHAR(255) NOT NULL,
      "status" tinyint NOT NULL,
      "preview" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_schedulers`
    --
     
    CREATE TABLE "generic_schedulers" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_items`
    --
     
    CREATE TABLE "generic_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "file" VARCHAR(255) NOT NULL,
      "type" tinyint NOT NULL,
      "code" ntext NOT NULL,
      "preview" VARCHAR(255) NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id")
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_scheduler_configs`
    --
     
    CREATE TABLE "generic_scheduler_configs" (
      "id" INT NOT NULL IDENTITY(1,1),
      "template_id" INT NOT NULL,
      "scheduler_id" INT NOT NULL,
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "startat" DATETIME DEFAULT (getdate()) NOT NULL,
      "endat" DATETIME DEFAULT (getdate()) NOT NULL,
      "monday" tinyint NOT NULL DEFAULT '1',
      "tuesday" tinyint NOT NULL DEFAULT '1',
      "wednesday" tinyint NOT NULL DEFAULT '1',
      "thursday" tinyint NOT NULL DEFAULT '1',
      "friday" tinyint NOT NULL DEFAULT '1',
      "saturday" tinyint NOT NULL DEFAULT '1',
      "sunday" tinyint NOT NULL DEFAULT '1',
      "starthour" INT NOT NULL,
      "endhour" INT NOT NULL,
      "startminute" INT NOT NULL,
      "endminute" INT NOT NULL,
      "startsecond" INT NOT NULL,
      "endsecond" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([template_id]) REFERENCES [generic_templates] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_scheduler_config_items`
    --
     
    CREATE TABLE "generic_scheduler_config_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "item_id" INT NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "priority" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([item_id]) REFERENCES [generic_items] ([id])
    );
     
    -- --------------------------------------------------------
    
    --
    -- Estrutura da tabela `generic_configs`
    --
     
    CREATE TABLE "generic_configs" (
      "id" INT NOT NULL IDENTITY(1,1),
      "template_id" INT NOT NULL,
      "scheduler_id" INT NOT NULL,
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "default" tinyint NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([template_id]) REFERENCES [generic_templates] ([id]),
      FOREIGN KEY ([scheduler_id]) REFERENCES [generic_schedulers] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_devices`
    --
     
    CREATE TABLE "generic_devices" (
      "id" INT NOT NULL IDENTITY(1,1),
      "config_id" INT NOT NULL,
      "title" VARCHAR(255) NOT NULL,
      "description" ntext NOT NULL,
      "location" VARCHAR(255) NOT NULL,
      "ip" VARCHAR(255) NOT NULL,
      "status" tinyint NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([config_id]) REFERENCES [generic_configs] ([id])
    );
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_config_items`
    --
     
    CREATE TABLE "generic_config_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "config_id" INT NOT NULL,
      "item_id" INT NOT NULL,
      "status" tinyint NOT NULL DEFAULT '1',
      "priority" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([config_id]) REFERENCES [generic_configs] ([id]),
      FOREIGN KEY ([item_id]) REFERENCES [generic_items] ([id])
    );
    
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_scheduler_items`
    --
     
    CREATE TABLE "generic_scheduler_items" (
      "id" INT NOT NULL IDENTITY(1,1),
      "scheduler_id" INT NOT NULL,
      "config_id" INT NOT NULL,
      "created_at" DATETIME DEFAULT (getdate()) NOT NULL,
      "updated_at" DATETIME DEFAULT (getdate()) NOT NULL,
      PRIMARY KEY NONCLUSTERED ("id"),
      FOREIGN KEY ([scheduler_id]) REFERENCES [generic_schedulers] ([id]),
      FOREIGN KEY ([config_id]) REFERENCES [generic_scheduler_configs] ([id])
    );
     
    -- --------------------------------------------------------
    
    -- --------------------------------------------------------
    
    --
    -- Seed
    --
    
    INSERT INTO [users] ([name], [email], [password]) VALUES ('admin', 'user@ua.pt', 'userpass');
    --INSERT INTO [kinect_templates] ([title], [description], [path], [preview]) VALUES ('Default', 'Default Template', '', '');