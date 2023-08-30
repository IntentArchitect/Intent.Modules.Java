package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnore;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentManage;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.Mode;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;

@IntentManage(annotations = Mode.Ignore)
@SpringBootApplication()
@EntityScan(basePackages = {"com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication", "za.myorg.mypackage"})
public class Application {
    public static void main(final String[] args) {
        SpringApplication.run(Application.class, args);
    }
}