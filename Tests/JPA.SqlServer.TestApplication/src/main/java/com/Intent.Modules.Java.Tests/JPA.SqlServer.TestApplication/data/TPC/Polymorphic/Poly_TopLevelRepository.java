package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.Polymorphic;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.Polymorphic.Poly_TopLevel;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface Poly_TopLevelRepository extends JpaRepository<Poly_TopLevel, UUID> {
}
