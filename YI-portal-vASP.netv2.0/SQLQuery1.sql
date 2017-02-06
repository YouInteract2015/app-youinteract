	

    -- phpMyAdmin SQL Dump
    -- version 2.11.11.3
    -- http://www.phpmyadmin.net
    --
    -- Máquina: localhost
    -- Data de Criação: 11-Mar-2015 às 17:34
    -- Versão do servidor: 5.5.40
    -- versão do PHP: 5.5.22
     
    --
    -- Base de Dados: `youinteract`
    --
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_configs`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_configs" (
      "id" INT(10) UNSIGNED NOT NULL,
      "template_id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "default" tinyint(1) NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "generic_configs_template_id_foreign" ("template_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_configs`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_config_items`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_config_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "config_id" INT(10) UNSIGNED NOT NULL,
      "item_id" INT(10) UNSIGNED NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "priority" INT(10) UNSIGNED NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "generic_config_items_config_id_foreign" ("config_id"),
      KEY "generic_config_items_item_id_foreign" ("item_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_config_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_devices`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_devices" (
      "id" INT(10) UNSIGNED NOT NULL,
      "config_id" INT(10) UNSIGNED NOT NULL,
      "scheduler_id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "location" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "ip" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "status" tinyint(1) NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "generic_devices_config_id_foreign" ("config_id"),
      KEY "generic_devices_scheduler_id_foreign" ("scheduler_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_devices`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_items`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "file" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "type" tinyint(1) NOT NULL,
      "code" text COLLATE utf8_unicode_ci NOT NULL,
      "preview" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_schedulers`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_schedulers" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_schedulers`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_scheduler_configs`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_scheduler_configs" (
      "id" INT(10) UNSIGNED NOT NULL,
      "template_id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "startat" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "endat" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "monday" tinyint(1) NOT NULL DEFAULT '1',
      "tuesday" tinyint(1) NOT NULL DEFAULT '1',
      "wednesday" tinyint(1) NOT NULL DEFAULT '1',
      "thursday" tinyint(1) NOT NULL DEFAULT '1',
      "friday" tinyint(1) NOT NULL DEFAULT '1',
      "saturday" tinyint(1) NOT NULL DEFAULT '1',
      "sunday" tinyint(1) NOT NULL DEFAULT '1',
      "starthour" INT(11) NOT NULL,
      "endhour" INT(11) NOT NULL,
      "startminute" INT(11) NOT NULL,
      "endminute" INT(11) NOT NULL,
      "startsecond" INT(11) NOT NULL,
      "endsecond" INT(11) NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "generic_scheduler_configs_template_id_foreign" ("template_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_scheduler_configs`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_scheduler_config_items`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_scheduler_config_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "item_id" INT(10) UNSIGNED NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "priority" INT(10) UNSIGNED NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "generic_scheduler_config_items_item_id_foreign" ("item_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_scheduler_config_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_scheduler_items`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_scheduler_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "scheduler_id" INT(10) UNSIGNED NOT NULL,
      "config_id" INT(10) UNSIGNED NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "generic_scheduler_items_scheduler_id_foreign" ("scheduler_id"),
      KEY "generic_scheduler_items_config_id_foreign" ("config_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_scheduler_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `generic_templates`
    --
     
    CREATE TABLE IF NOT EXISTS "generic_templates" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "view" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "status" tinyint(1) NOT NULL,
      "preview" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `generic_templates`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_configs`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_configs" (
      "id" INT(10) UNSIGNED NOT NULL,
      "template_id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "default" tinyint(1) NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "kinect_configs_template_id_foreign" ("template_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_configs`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_config_items`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_config_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "config_id" INT(10) UNSIGNED NOT NULL,
      "item_id" INT(10) UNSIGNED NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "priority" INT(10) UNSIGNED NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "kinect_config_items_config_id_foreign" ("config_id"),
      KEY "kinect_config_items_item_id_foreign" ("item_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_config_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_devices`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_devices" (
      "id" INT(10) UNSIGNED NOT NULL,
      "config_id" INT(10) UNSIGNED NOT NULL,
      "scheduler_id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "location" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "ip" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "status" tinyint(1) NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "kinect_devices_config_id_foreign" ("config_id"),
      KEY "kinect_devices_scheduler_id_foreign" ("scheduler_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_devices`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_items`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "dll" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "config" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "version" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_items_download`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_items_download" (
      "id" INT(10) UNSIGNED NOT NULL,
      "device_id" INT(10) UNSIGNED NOT NULL,
      "item_id" INT(10) UNSIGNED NOT NULL,
      "version" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "kinect_items_download_device_id_foreign" ("device_id"),
      KEY "kinect_items_download_item_id_foreign" ("item_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_items_download`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_schedulers`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_schedulers" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "default" tinyint(1) NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_schedulers`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_scheduler_configs`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_scheduler_configs" (
      "id" INT(10) UNSIGNED NOT NULL,
      "item_id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "startat" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "endat" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "monday" tinyint(1) NOT NULL DEFAULT '1',
      "tuesday" tinyint(1) NOT NULL DEFAULT '1',
      "wednesday" tinyint(1) NOT NULL DEFAULT '1',
      "thursday" tinyint(1) NOT NULL DEFAULT '1',
      "friday" tinyint(1) NOT NULL DEFAULT '1',
      "saturday" tinyint(1) NOT NULL DEFAULT '1',
      "sunday" tinyint(1) NOT NULL DEFAULT '1',
      "starthour" INT(11) NOT NULL,
      "endhour" INT(11) NOT NULL,
      "startminute" INT(11) NOT NULL,
      "endminute" INT(11) NOT NULL,
      "startsecond" INT(11) NOT NULL,
      "endsecond" INT(11) NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "kinect_scheduler_configs_item_id_foreign" ("item_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_scheduler_configs`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_scheduler_config_items`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_scheduler_config_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "file" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "preview" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_scheduler_config_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_scheduler_items`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_scheduler_items" (
      "id" INT(10) UNSIGNED NOT NULL,
      "config_id" INT(10) UNSIGNED NOT NULL,
      "scheduler_id" INT(10) UNSIGNED NOT NULL,
      "status" tinyint(1) NOT NULL DEFAULT '1',
      "priority" INT(11) NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      KEY "kinect_scheduler_items_config_id_foreign" ("config_id"),
      KEY "kinect_scheduler_items_scheduler_id_foreign" ("scheduler_id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_scheduler_items`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `kinect_templates`
    --
     
    CREATE TABLE IF NOT EXISTS "kinect_templates" (
      "id" INT(10) UNSIGNED NOT NULL,
      "title" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "description" text COLLATE utf8_unicode_ci NOT NULL,
      "path" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "default" tinyint(1) NOT NULL,
      "preview" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `kinect_templates`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `migrations`
    --
     
    CREATE TABLE IF NOT EXISTS "migrations" (
      "migration" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "batch" INT(11) NOT NULL
    );
     
    --
    -- Extraindo dados da tabela `migrations`
    --
     
    INSERT INTO `migrations` (`migration`, `batch`) VALUES
    ('2014_10_12_000000_create_users_table', 1),
    ('2014_10_12_100000_create_password_resets_table', 1),
    ('2015_03_05_020101_create_kinect_templates_table', 1),
    ('2015_03_05_020102_create_kinect_schedulers_table', 1),
    ('2015_03_05_020103_create_kinect_items_table', 1),
    ('2015_03_05_020104_create_kinect_configs_table', 1),
    ('2015_03_05_020105_create_kinect_config_items_table', 1),
    ('2015_03_05_020106_create_kinect_devices_table', 1),
    ('2015_03_05_020107_create_kinect_scheduler_config_items_table', 1),
    ('2015_03_05_020108_create_kinect_scheduler_configs_table', 1),
    ('2015_03_05_020109_create_kinect_scheduler_items_table', 1),
    ('2015_03_05_020110_create_kinect_items_download_table', 1),
    ('2015_03_06_020101_create_generic_templates_table', 1),
    ('2015_03_06_020102_create_generic_schedulers_table', 1),
    ('2015_03_06_020103_create_generic_items_table', 1),
    ('2015_03_06_020104_create_generic_configs_table', 1),
    ('2015_03_06_020105_create_generic_scheduler_configs_table', 1),
    ('2015_03_06_020106_create_generic_scheduler_config_items_table', 1),
    ('2015_03_06_020107_create_generic_devices_table', 1),
    ('2015_03_06_020108_create_generic_config_items_table', 1),
    ('2015_03_06_020109_create_generic_scheduler_items_table', 1);
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `password_resets`
    --
     
    CREATE TABLE IF NOT EXISTS "password_resets" (
      "email" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "token" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      KEY "password_resets_email_index" ("email"),
      KEY "password_resets_token_index" ("token")
    );
     
    --
    -- Extraindo dados da tabela `password_resets`
    --
     
     
    -- --------------------------------------------------------
     
    --
    -- Estrutura da tabela `users`
    --
     
    CREATE TABLE IF NOT EXISTS "users" (
      "id" INT(10) UNSIGNED NOT NULL,
      "name" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "email" VARCHAR(255) COLLATE utf8_unicode_ci NOT NULL,
      "password" VARCHAR(60) COLLATE utf8_unicode_ci NOT NULL,
      "remember_token" VARCHAR(100) COLLATE utf8_unicode_ci DEFAULT NULL,
      "created_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      "updated_at" TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
      PRIMARY KEY ("id"),
      UNIQUE KEY "users_email_unique" ("email")
    ) AUTO_INCREMENT=1 ;
     
    --
    -- Extraindo dados da tabela `users`
    --
     
     
    --
    -- Constraints for dumped tables
    --
     
    --
    -- Limitadores para a tabela `generic_configs`
    --
    ALTER TABLE `generic_configs`
      ADD CONSTRAINT "generic_configs_template_id_foreign" FOREIGN KEY ("template_id") REFERENCES "generic_templates" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `generic_config_items`
    --
    ALTER TABLE `generic_config_items`
      ADD CONSTRAINT "generic_config_items_item_id_foreign" FOREIGN KEY ("item_id") REFERENCES "generic_items" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "generic_config_items_config_id_foreign" FOREIGN KEY ("config_id") REFERENCES "generic_configs" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `generic_devices`
    --
    ALTER TABLE `generic_devices`
      ADD CONSTRAINT "generic_devices_scheduler_id_foreign" FOREIGN KEY ("scheduler_id") REFERENCES "generic_schedulers" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "generic_devices_config_id_foreign" FOREIGN KEY ("config_id") REFERENCES "generic_configs" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `generic_scheduler_configs`
    --
    ALTER TABLE `generic_scheduler_configs`
      ADD CONSTRAINT "generic_scheduler_configs_template_id_foreign" FOREIGN KEY ("template_id") REFERENCES "generic_templates" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `generic_scheduler_config_items`
    --
    ALTER TABLE `generic_scheduler_config_items`
      ADD CONSTRAINT "generic_scheduler_config_items_item_id_foreign" FOREIGN KEY ("item_id") REFERENCES "generic_items" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `generic_scheduler_items`
    --
    ALTER TABLE `generic_scheduler_items`
      ADD CONSTRAINT "generic_scheduler_items_config_id_foreign" FOREIGN KEY ("config_id") REFERENCES "generic_scheduler_configs" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "generic_scheduler_items_scheduler_id_foreign" FOREIGN KEY ("scheduler_id") REFERENCES "generic_schedulers" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `kinect_configs`
    --
    ALTER TABLE `kinect_configs`
      ADD CONSTRAINT "kinect_configs_template_id_foreign" FOREIGN KEY ("template_id") REFERENCES "kinect_templates" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `kinect_config_items`
    --
    ALTER TABLE `kinect_config_items`
      ADD CONSTRAINT "kinect_config_items_item_id_foreign" FOREIGN KEY ("item_id") REFERENCES "kinect_items" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "kinect_config_items_config_id_foreign" FOREIGN KEY ("config_id") REFERENCES "kinect_configs" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `kinect_devices`
    --
    ALTER TABLE `kinect_devices`
      ADD CONSTRAINT "kinect_devices_scheduler_id_foreign" FOREIGN KEY ("scheduler_id") REFERENCES "kinect_schedulers" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "kinect_devices_config_id_foreign" FOREIGN KEY ("config_id") REFERENCES "kinect_configs" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `kinect_items_download`
    --
    ALTER TABLE `kinect_items_download`
      ADD CONSTRAINT "kinect_items_download_item_id_foreign" FOREIGN KEY ("item_id") REFERENCES "kinect_items" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "kinect_items_download_device_id_foreign" FOREIGN KEY ("device_id") REFERENCES "kinect_devices" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `kinect_scheduler_configs`
    --
    ALTER TABLE `kinect_scheduler_configs`
      ADD CONSTRAINT "kinect_scheduler_configs_item_id_foreign" FOREIGN KEY ("item_id") REFERENCES "kinect_scheduler_config_items" ("id") ON DELETE CASCADE;
     
    --
    -- Limitadores para a tabela `kinect_scheduler_items`
    --
    ALTER TABLE `kinect_scheduler_items`
      ADD CONSTRAINT "kinect_scheduler_items_scheduler_id_foreign" FOREIGN KEY ("scheduler_id") REFERENCES "kinect_schedulers" ("id") ON DELETE CASCADE,
      ADD CONSTRAINT "kinect_scheduler_items_config_id_foreign" FOREIGN KEY ("config_id") REFERENCES "kinect_scheduler_configs" ("id") ON DELETE CASCADE;

