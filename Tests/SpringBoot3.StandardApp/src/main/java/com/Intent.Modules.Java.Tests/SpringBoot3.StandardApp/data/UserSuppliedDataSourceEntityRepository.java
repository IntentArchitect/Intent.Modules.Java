package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.UserSuppliedDataSourceEntity;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentMerge;

@IntentMerge
public interface UserSuppliedDataSourceEntityRepository extends JpaRepository<UserSuppliedDataSourceEntity, Long> {
}
