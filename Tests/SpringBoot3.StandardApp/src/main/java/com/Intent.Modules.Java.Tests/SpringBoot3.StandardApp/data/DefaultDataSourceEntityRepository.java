package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.DefaultDataSourceEntity;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentMerge;

@IntentMerge
public interface DefaultDataSourceEntityRepository extends JpaRepository<DefaultDataSourceEntity, Long> {
}
