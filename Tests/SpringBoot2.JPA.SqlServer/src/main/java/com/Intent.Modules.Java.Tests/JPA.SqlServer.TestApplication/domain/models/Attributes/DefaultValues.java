package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Attributes;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.OffsetDateTime;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Enumerated;
import javax.persistence.EnumType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import org.hibernate.annotations.ColumnDefault;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "default_values")
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class DefaultValues implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
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
    @ColumnDefault("1")
    @Column(name = "bool_default1", nullable = false)
    private Boolean boolDefault1 = true;

    @NotNull
    @ColumnDefault("0")
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
    @ColumnDefault("1")
    @Enumerated(EnumType.ORDINAL)
    @Column(name = "ord_enum", nullable = false)
    private OrdinalEnumerated ordEnum = OrdinalEnumerated.LITERAL_TWO;

    public boolean isNew() {
        return this.id == null;
    }
}