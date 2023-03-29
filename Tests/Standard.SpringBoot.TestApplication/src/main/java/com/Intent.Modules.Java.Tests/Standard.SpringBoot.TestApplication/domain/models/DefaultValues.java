package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.Mode;
import java.io.Serializable;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.OffsetDateTime;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Enumerated;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import org.hibernate.annotations.ColumnDefault;
import javax.persistence.EnumType;

@Entity
@Table(name = "default_values")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class DefaultValues implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @ColumnDefault("'my default string'")
    @Column(name = "string_default", nullable = false)
    private String stringDefault = "my default string";

    @NotNull
    @ColumnDefault("42")
    @Column(name = "int_default", nullable = false)
    private Integer intDefault = 42;

    @NotNull
    @ColumnDefault("true")
    @Column(name = "bool_default1", nullable = false)
    private Boolean boolDefault1 = true;

    @NotNull
    @ColumnDefault("false")
    @Column(name = "bool_default2", nullable = false)
    private Boolean boolDefault2 = false;

    @NotNull
    @ColumnDefault("Current_Timestamp")
    @Column(name = "timestamp", nullable = false)
    private LocalDateTime timestamp = LocalDateTime.now();

    @NotNull
    @ColumnDefault("Current_Timestamp")
    @Column(name = "datetimeoffset", nullable = false)
    private OffsetDateTime datetimeoffset = OffsetDateTime.now();

    @NotNull
    @ColumnDefault("Current_Timestamp")
    @Column(name = "dateonly", nullable = false)
    private LocalDate dateonly = LocalDate.now();

    @NotNull
    @ColumnDefault("'2'")
    @Enumerated(EnumType.STRING)
    @Column(name = "str_enum", nullable = false)
    private StringEnumerated strEnum = StringEnumerated.VALUE_TWO;

    @NotNull
    @Enumerated(EnumType.ORDINAL)
    @Column(name = "ord_enum", nullable = false)
    private OrdinalEnumerated ordEnum;

    public boolean isNew() {
        return this.id == null;
    }
}