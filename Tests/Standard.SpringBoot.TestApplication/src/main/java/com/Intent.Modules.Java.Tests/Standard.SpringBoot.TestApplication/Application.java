package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnore;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentManage;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.Mode;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Application {
    public static void main(final String[] args) {
        SpringApplication.run(Application.class, args);
    }
}