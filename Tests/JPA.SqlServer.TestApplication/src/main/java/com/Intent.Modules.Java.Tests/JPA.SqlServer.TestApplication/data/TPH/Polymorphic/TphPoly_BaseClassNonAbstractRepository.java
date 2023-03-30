package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPH.Polymorphic;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.Polymorphic.TphPoly_BaseClassNonAbstract;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface TphPoly_BaseClassNonAbstractRepository extends JpaRepository<TphPoly_BaseClassNonAbstract, UUID> {
}