package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.data.projections.OrderQueryProjection;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentMerge;
import java.util.Optional;
import java.util.UUID;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import za.myorg.mypackage.Order;

@IntentMerge
public interface OrderRepository extends JpaRepository<Order, UUID> {
    @Query("select " +
                "ord.number as number, " +
                "ord.id as id " +
                "from Order ord join ord.orderItems orderItem " +
                "where orderItem.description = :description")
        Optional<OrderQueryProjection> getOrderByDescription(@Param("description") String description);
}
