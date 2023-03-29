package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.DefaultValues;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface DefaultValuesRepository extends JpaRepository<DefaultValues, UUID> {
}
