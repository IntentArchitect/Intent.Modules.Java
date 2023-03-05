package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys;

import java.io.Serializable;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import java.util.UUID;

@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
@EqualsAndHashCode
public class PK_A_CompositeKeyId implements Serializable {
    private UUID compositeKeyA;

    private UUID compositeKeyB;
}
